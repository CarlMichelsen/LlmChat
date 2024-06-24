using Domain.Abstraction;
using Domain.Entity.Id;

namespace Interface.Repository;

public interface IConversationDeletionRepository
{
    Task<Result<bool>> DeleteConversation(ConversationEntityId conversationEntityId);
}
