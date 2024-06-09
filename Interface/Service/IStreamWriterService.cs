using Domain.Dto.Chat.Stream;
using Domain.Entity.Id;
using Domain.Exception;

namespace Interface.Service;

public interface IStreamWriterService
{
    Task WriteContentDelta(ContentDeltaDto contentDeltaDto);

    Task WriteConclusion(ContentConcludedDto contentConcludedDto);

    Task WriteIds(ConversationEntityId? conversationEntityId, MessageEntityId? messageEntityId);

    Task<SafeUserFeedbackException> WriteError(string error, Exception? potentialSafeUserException = default);
}
