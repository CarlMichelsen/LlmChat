using Domain.Dto;
using Domain.Dto.Conversation;

namespace Interface.Handler;

public interface IConversationHandler
{
    Task<ServiceResponse<List<ConversationOptionDto>>> GetConversationList();

    Task<ServiceResponse<object>> GetConversation();
}
