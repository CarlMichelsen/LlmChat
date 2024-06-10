using Domain.Abstraction;
using Domain.Entity.Id;

namespace Interface.Repository;

public interface IConversationSystemMessageRepository
{
    Task<Result<string>> SetConversationSystemMessage(
        ConversationEntityId conversationId,
        string systemMessage);
}
