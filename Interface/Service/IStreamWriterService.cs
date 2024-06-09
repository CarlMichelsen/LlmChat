using Domain.Dto.Chat.Stream;
using Domain.Entity.Id;
using Domain.Exception;

namespace Interface.Service;

public interface IStreamWriterService
{
    Task WriteContentDelta(ContentDeltaDto contentDeltaDto);

    Task WriteConclusion(ConversationEntityId conversationId, ContentConcludedDto contentConcludedDto);
    
    Task WriteSummary(ConversationEntityId conversationId, string summary);

    Task WriteIds(ConversationEntityId conversationEntityId, MessageEntityId? messageEntityId);

    Task<SafeUserFeedbackException> WriteError(string error, Exception? potentialSafeUserException = default);
}
