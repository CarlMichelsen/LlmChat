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
        var promptResult = PromptMapper.ToPrompt(newUserMessageDto);

        if (promptResult.IsError)
        {
            await this.RespondWithError(default, MapError(promptResult.Error!, "Failed to map to prompt"));
            return;
        }

        var model = await modelService.GetModel(promptResult.Unwrap().ModelIdentifier);
        if (model is null)
        {
            await this.RespondWithError(default, "Failed to find model");
            return;
        }

        var conversationResult = await conversationRepository.GetOrCreateConversation(
            sessionService.UserProfileId,
            newUserMessageDto.ConversationId);

        if (conversationResult.IsError)
        {
            await this.RespondWithError(default, MapError(conversationResult.Error!, "Failed to map to prompt"));
            return;
        }

        var conversation = conversationResult.Unwrap();
        newUserMessageDto = newUserMessageDto with { ConversationId = conversation.Id };
        await this.RespondWithNewConversationId(conversation.Id);

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

        var parser = new LargeLanguageModelClientParseService(model) as ILargeLanguageModelClientParseService;
        var parsedStream = parser.Parse(
            initiatedMessageResult.Unwrap(),
            largeLanguageModelClient.PromptStream(promptResult.Unwrap(), cancellationToken),
            this.ConcludeMessage);

        var context = httpContextAccessor.HttpContext!;
        await foreach (var streamEvent in parsedStream)
        {
            await context.Response.WriteAsync("\n", CancellationToken.None);
            await context.Response.WriteAsync(JsonSerializer.Serialize(streamEvent), CancellationToken.None);
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
            ConversationId: conclusion.NewMessageData.Conversation.Id,
            ResponseToMessageId: conclusion.NewMessageData.Message.Id,
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

        var completedConversation = assistantResponseResult.Unwrap().Conversation;
        var summaryResult = await summaryService.GenerateAndApplySummary(completedConversation);
        if (summaryResult.Error is not null)
        {
            await this.RespondWithError(default, MapError(summaryResult.Error!, "Failed to generate conversation summary"));
            return;
        }

        await this.RespondWithNewConversationId(completedConversation.Id, summaryResult.Unwrap());
    }

    private async Task RespondWithError(long? conversationId, string error)
    {
        var context = httpContextAccessor.HttpContext!;
        var obj = new ContentDeltaDto(
            ConversationId: conversationId?.ToString(),
            Content: default,
            Concluded: default,
            Summary: default,
            Error: error);
        
        await context.Response.WriteAsync("\n", CancellationToken.None);
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(obj),
            CancellationToken.None);
    }

    private async Task RespondWithNewConversationId(long conversationId, string? summary = default)
    {
        var context = httpContextAccessor.HttpContext!;
        await context.Response.WriteAsync("\n", CancellationToken.None);
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new ContentDeltaDto(
                ConversationId: conversationId.ToString(),
                Content: default,
                Concluded: default,
                Summary: summary,
                Error: default)),
            CancellationToken.None);
    }
}
