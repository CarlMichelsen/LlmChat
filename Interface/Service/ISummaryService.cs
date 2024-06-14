using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;

namespace Interface.Service;

public interface ISummaryService
{
    Task<Result<string>> GenerateSummary(ConversationEntity conversationEntity, MessageEntityId fromMessageId);
}
