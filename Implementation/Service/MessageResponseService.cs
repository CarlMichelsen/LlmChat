using System.Text.Json;
using Domain.Conversation;
using Domain.Dto.Chat;
using Domain.Dto.Chat.Stream;
using Domain.Exception;
using Implementation.Map;
using Interface.Repository;
using Interface.Service;
using LargeLanguageModelClient;
using LargeLanguageModelClient.Dto.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Implementation.Service;

public class MessageResponseService(
    ILogger<MessageResponseService> logger,
    ILargeLanguageModelClient largeLanguageModelClient,
    IConversationRepository conversationRepository,
    IModelService modelService,
    ISummaryService summaryService,
    ISessionService sessionService,
    IHttpContextAccessor httpContextAccessor) : IMessageResponseService
{
    public async Task Respond(
        NewMessageDto newUserMessageDto,
        CancellationToken cancellationToken)
    {
        var parsedConversationId = long.TryParse(newUserMessageDto.ConversationId, out long convId);
        if (!parsedConversationId && !string.IsNullOrWhiteSpace(newUserMessageDto.ConversationId))
        {
            await this.RespondWithError(default, "Failed to parse conversationId");
        }
        
        long? conversationId = parsedConversationId ? convId : null;

        var model = await modelService.GetModel(newUserMessageDto.ModelIdentifier);
        if (model is null)
        {
            await this.RespondWithError(default, "Failed to find model");
            return;
        }

        var conversationResult = await conversationRepository.GetOrCreateConversation(
            sessionService.UserProfileId,
            conversationId);

        if (conversationResult.IsError)
        {
            await this.RespondWithError(default, MapError(conversationResult.Error!, "Failed to map to prompt"));
            return;
        }

        var conversation = conversationResult.Unwrap();
        var promptResult = PromptMapper.ToPrompt(conversation, newUserMessageDto);
        if (promptResult.IsError)
        {
            await this.RespondWithError(default, MapError(promptResult.Error!, "Failed to map to prompt"));
            return;
        }

        newUserMessageDto = newUserMessageDto with { ConversationId = conversation.Id.ToString() };
        await this.RespondWithMetaData(conversation.Id.ToString());

        var initiatedMessageResult = await conversationRepository.InitiateMessage(
            newUserMessageDto,
            model,
            conversation,
            default);
        if (initiatedMessageResult.IsError)
        {
            await this.RespondWithError(default, MapError(initiatedMessageResult.Error!, "Failed to initiate message"));
            return;
        }

        var userMessage = initiatedMessageResult.Unwrap();
        await this.RespondWithMetaData(userMessage.Conversation.Id.ToString(), conversation.Summary, userMessage.Message.Id.ToString());

        var parser = new LargeLanguageModelClientParseService(model) as ILargeLanguageModelClientParseService;
        var parsedStream = parser.Parse(
            userMessage,
            largeLanguageModelClient.PromptStream(promptResult.Unwrap(), cancellationToken),
            this.ConcludeMessage);

        var context = httpContextAccessor.HttpContext!;
        await foreach (var streamEvent in parsedStream)
        {
            await context.Response.WriteAsync(JsonSerializer.Serialize(streamEvent), CancellationToken.None);
            await context.Response.WriteAsync("\n", CancellationToken.None);
        }
    }

    private static string MapError(Exception e, string initial)
    {
        if (e is SafeUserFeedbackException safe)
        {
            return $"{initial} -> {safe.Message}";
        }

        return initial;
    }

    private async Task ConcludeMessage(ConcludedMessage conclusion, LlmModelDto model)
    {
        var message = new NewMessageDto(
            ConversationId: conclusion.NewMessageData.Conversation.Id.ToString(),
            ResponseToMessageId: conclusion.NewMessageData.Message.Id.ToString(),
            Content: conclusion.Content,
            ModelIdentifier: model.Id);

        var assistantResponseResult = await conversationRepository.InitiateMessage(
            message,
            model,
            conclusion.NewMessageData.Conversation,
            conclusion.StreamUsage);
        
        if (assistantResponseResult.IsError)
        {
            logger.LogCritical(assistantResponseResult.Error!, "Failed to respond to user message");
        }

        var newMessage = assistantResponseResult.Unwrap();
        var concluded = new ContentConcludedDto
        {
            ModelName = model.ModelIdentifierName,
            ModelId = model.Id,
            ProviderPromptIdentifier = conclusion.ProviderPromptIdentifier,
            InputTokens = (int)conclusion.InputTokens,
            OutputTokens = (int)conclusion.OutputTokens,
            StopReason = conclusion.StreamUsage?.StopReason ?? "unknown",
            MessageId = newMessage.Message.Id.ToString(),
        };
        await this.RespondWithConclusion(newMessage.Conversation.Id.ToString(), concluded);

        if (string.IsNullOrWhiteSpace(newMessage.Conversation.Summary))
        {
            var summaryResult = await summaryService.GenerateAndApplySummary(newMessage.Conversation);
            if (summaryResult.Error is not null)
            {
                await this.RespondWithError(newMessage.Conversation.Id.ToString(), MapError(summaryResult.Error!, "Failed to generate conversation summary"));
                return;
            }

            await this.RespondWithMetaData(newMessage.Conversation.Id.ToString(), summaryResult.Unwrap());
        }
    }

    private async Task RespondWithError(string? conversationId, string error)
    {
        var context = httpContextAccessor.HttpContext!;
        var obj = new ContentDeltaDto(
            ConversationId: conversationId,
            UserMessageId: default,
            Content: default,
            Concluded: default,
            Summary: default,
            Error: error);
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(obj), CancellationToken.None);
        await context.Response.WriteAsync("\n", CancellationToken.None);
    }

    private async Task RespondWithMetaData(string conversationId, string? summary = default, string? userMessageId = default)
    {
        var context = httpContextAccessor.HttpContext!;
        var content = new ContentDeltaDto(
            ConversationId: conversationId,
            UserMessageId: userMessageId,
            Content: default,
            Concluded: default,
            Summary: summary,
            Error: default);
            
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(content),
            CancellationToken.None);
        await context.Response.WriteAsync("\n", CancellationToken.None);
    }

    private async Task RespondWithConclusion(string conversationId, ContentConcludedDto contentConcludedDto)
    {
        var context = httpContextAccessor.HttpContext!;
        var content = new ContentDeltaDto(
            ConversationId: conversationId,
            UserMessageId: default,
            Content: default,
            Concluded: contentConcludedDto,
            Summary: default,
            Error: default);
            
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(content),
            CancellationToken.None);
        await context.Response.WriteAsync("\n", CancellationToken.None);
    }
}
