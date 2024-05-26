using System.Text.Json;
using Domain.Conversation;
using Domain.Dto.Chat;
using Domain.Dto.Chat.Stream;
using Domain.Exception;
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
    IPromptMapperService promptMapperService,
    IGetOrCreateConversationRepository getOrCreateConversationRepository,
    IMessageInitiationRepository messageInitiationRepository,
    ISessionService sessionService,
    IHttpContextAccessor httpContextAccessor) : IMessageResponseService
{
    public async Task Respond(
        NewMessageDto newUserMessageDto,
        CancellationToken cancellationToken)
    {
        var promptResult = await promptMapperService
            .ToPrompt(newUserMessageDto);

        if (promptResult.IsError)
        {
            await this.RespondWithError(default, MapError(promptResult.Error!, "Failed to map to prompt"));
            return;
        }

        var modelResponse = await largeLanguageModelClient.GetModel(
            promptResult.Unwrap().ModelIdentifier);
        if (!modelResponse.Ok)
        {
            await this.RespondWithError(default, string.Join(", ", modelResponse.Errors));
            return;
        }

        var conversationResult = await getOrCreateConversationRepository.GetOrCreateConversation(
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

        var initiatedMessageResult = await messageInitiationRepository.InitiateMessage(
            newUserMessageDto,
            modelResponse.Data!,
            conversation,
            default);
        if (initiatedMessageResult.IsError)
        {
            await this.RespondWithError(default, MapError(initiatedMessageResult.Error!, "Failed to initiate message"));
            return;
        }

        var parser = new LargeLanguageModelClientParseService(modelResponse.Data!) as ILargeLanguageModelClientParseService;
        var parsedStream = parser.Parse(
            initiatedMessageResult.Unwrap(),
            modelResponse.Data!,
            largeLanguageModelClient.PromptStream(promptResult.Unwrap(), cancellationToken),
            this.ConcludeMessage);

        var context = httpContextAccessor.HttpContext!;
        await foreach (var streamEvent in parsedStream)
        {
            await context.Response.WriteAsync("\n", CancellationToken.None);
            await context.Response.WriteAsync(JsonSerializer.Serialize(streamEvent), CancellationToken.None);
        }
    }

    private static string MapError(Exception e, string fallback)
    {
        if (e is SafeUserFeedbackException safe)
        {
            return safe.Message;
        }

        return fallback;
    }

    private async Task ConcludeMessage(ConcludedMessage conclusion, LlmModelDto model)
    {
        var message = new NewMessageDto(
            ConversationId: conclusion.NewMessageData.Conversation.Id,
            ResponseToMessageId: conclusion.NewMessageData.Message.Id,
            Content: conclusion.Content,
            ModelIdentifier: model.Id);

        var assistantResponseResult = await messageInitiationRepository.InitiateMessage(
            message,
            model,
            conclusion.NewMessageData.Conversation,
            conclusion.StreamUsage);
        
        if (assistantResponseResult.IsError)
        {
            logger.LogCritical(assistantResponseResult.Error!, "Failed to respond to user message");
        }
    }

    private async Task RespondWithError(long? conversationId, string error)
    {
        var context = httpContextAccessor.HttpContext!;
        var obj = new ContentDeltaDto(
            conversationId?.ToString(),
            default,
            default,
            error);
        
        await context.Response.WriteAsync("\n", CancellationToken.None);
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(obj),
            CancellationToken.None);
    }

    private async Task RespondWithNewConversationId(long conversationId)
    {
        var context = httpContextAccessor.HttpContext!;
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new ContentDeltaDto(
                conversationId.ToString(),
                default,
                default,
                default)),
            CancellationToken.None);
    }
}
