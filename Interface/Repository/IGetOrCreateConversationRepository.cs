using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;

namespace Interface.Repository;

public interface IGetOrCreateConversationRepository
{
    Task<Result<ConversationEntity>> GetOrCreateConversation(Guid creatorIdentifier, ConversationEntityId? conversationId);
}
