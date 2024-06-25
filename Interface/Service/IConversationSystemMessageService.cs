using Domain.Abstraction;
using Domain.Entity.Id;

namespace Interface.Service;

public interface IConversationSystemMessageService
{
    Task<Result<string>> SetConversationSystemMessage(
        ConversationEntityId conversationId,
        string systemMessage);
}
