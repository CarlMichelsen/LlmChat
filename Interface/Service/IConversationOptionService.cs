using Domain.Abstraction;
using Domain.Dto.Conversation;

namespace Interface.Service;

public interface IConversationOptionService
{
    Task<Result<List<ConversationOptionDto>>> GetConversationOptions(int amount);
}
