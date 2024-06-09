using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;

namespace Interface.Repository;

public interface IConversationReadRepository
{
    Task<Result<ConversationEntity>> GetRichConversation(Guid creatorIdentifier, ConversationEntityId conversationId);

    Task<Result<List<ConversationEntity>>> GetShallowConversations(Guid creatorIdentifier, int amount);
}
