using Domain.Dto;
using Domain.Dto.Conversation;
using Domain.Entity.Id;

namespace Interface.Handler;

public interface IConversationHandler
{
    Task<ServiceResponse<ConversationDto>> GetConversation(ConversationEntityId conversationId);
    
    Task<ServiceResponse<List<ConversationOptionDto>>> GetConversationList();
}
