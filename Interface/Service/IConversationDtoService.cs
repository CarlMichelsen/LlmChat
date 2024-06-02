using Domain.Abstraction;
using Domain.Dto.Conversation;

namespace Interface.Service;

public interface IConversationDtoService
{
    Task<Result<ConversationDto>> GetConversationDto(long conversationId);
}
