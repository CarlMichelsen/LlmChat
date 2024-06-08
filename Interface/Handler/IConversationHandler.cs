using Domain.Dto;
using Domain.Dto.Conversation;

namespace Interface.Handler;

public interface IConversationHandler
{
    Task<ServiceResponse<ConversationDto>> GetConversation(long conversationId);
    
    Task<ServiceResponse<List<ConversationOptionDto>>> GetConversationList();
}
