using Domain.Abstraction;
using Domain.Dto.Conversation;
using Domain.Entity.Id;

namespace Interface.Service;

public interface IConversationDtoService
{
    Task<Result<ConversationDto>> GetConversationDto(ConversationEntityId conversationId);
}
