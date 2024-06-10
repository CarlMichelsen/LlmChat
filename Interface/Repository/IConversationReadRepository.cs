using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;

namespace Interface.Repository;

public interface IConversationReadRepository
{
    Task<Result<ConversationEntity>> GetRichConversation(ProfileEntityId creatorIdentifier, ConversationEntityId conversationId);

    Task<Result<List<ConversationEntity>>> GetShallowConversations(ProfileEntityId creatorIdentifier, int amount);
}
