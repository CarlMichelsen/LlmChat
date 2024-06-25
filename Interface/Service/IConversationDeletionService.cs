using Domain.Abstraction;
using Domain.Entity.Id;

namespace Interface.Service;

public interface IConversationDeletionService
{
    Task<Result<bool>> DeleteConversation(ConversationEntityId conversationEntityId);
}
