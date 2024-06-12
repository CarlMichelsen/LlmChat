using Domain.Abstraction;
using Domain.Entity;

namespace Interface.Service;

public interface ISummaryService
{
    Task<Result<string>> GenerateSummary(ConversationEntity conversationEntity);
}
