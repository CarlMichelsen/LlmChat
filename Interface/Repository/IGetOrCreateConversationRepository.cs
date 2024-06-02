using Domain.Abstraction;
using Domain.Entity;

namespace Interface.Repository;

public interface IGetOrCreateConversationRepository
{
    Task<Result<ConversationEntity>> GetOrCreateConversation(Guid creatorIdentifier, long? conversationId);
}
