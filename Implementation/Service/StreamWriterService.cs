using System.Text.Json;
using Domain.Dto.Chat.Stream;
using Domain.Entity.Id;
using Domain.Exception;
using Interface.Service;
using Microsoft.AspNetCore.Http;

namespace Implementation.Service;

public class StreamWriterService(
    IHttpContextAccessor httpContextAccessor) : IStreamWriterService
{
    private HttpContext Context => httpContextAccessor.HttpContext
        ?? throw new Exception("Failed to get HttpContext in StreamWriterService");

    public Task WriteConclusion(ContentConcludedDto contentConcludedDto)
    {
        var content = new ContentDeltaDto(
            UserMessageId: default,
            ConversationId: default,
            Content: default,
            Concluded: contentConcludedDto,
            Summary: default,
            Error: default);
        return this.WriteContentDelta(content);
    }

    public async Task WriteIds(ConversationEntityId? conversationEntityId, MessageEntityId? messageEntityId)
    {
        var content = new ContentDeltaDto(
            UserMessageId: messageEntityId?.Value.ToString(),
            ConversationId: conversationEntityId?.Value.ToString(),
            Content: default,
            Concluded: default,
            Summary: default,
            Error: default);
        await this.WriteContentDelta(content);
    }

    public async Task<SafeUserFeedbackException> WriteError(
        string error,
        Exception? potentialSafeUserException = default)
    {
        string fullError;
        if (potentialSafeUserException is SafeUserFeedbackException safe)
        {
            fullError = $"{error} -> {safe.Details}";
        }
        else
        {
            fullError = error;
        }

        var content = new ContentDeltaDto(
            UserMessageId: default,
            ConversationId: default,
            Content: default,
            Concluded: default,
            Summary: default,
            Error: fullError);
        await this.WriteContentDelta(content);
        return new SafeUserFeedbackException(fullError);
    }

    public async Task WriteContentDelta(ContentDeltaDto contentDeltaDto)
    {
        await this.Context.Response.WriteAsync(JsonSerializer.Serialize(contentDeltaDto), CancellationToken.None);
        await this.Context.Response.WriteAsync(string.Empty, CancellationToken.None);
    }
}
