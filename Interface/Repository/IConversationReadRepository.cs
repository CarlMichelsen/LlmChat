using Domain.Abstraction;
using Domain.Entity;

namespace Interface.Repository;

public interface IConversationReadRepository
{
    Task<Result<ConversationEntity>> GetRichConversation(Guid creatorIdentifier, long conversationId);

    Task<Result<List<ConversationEntity>>> GetShallowConversations(Guid creatorIdentifier, int amount);
}
