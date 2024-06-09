using Domain.Dto.Conversation;
using Domain.Entity;
using Domain.Entity.Id;

namespace Interface.Service;

public interface IConversationDtoCacheService
{
    Task<ConversationDto?> GetConversationDto(ConversationEntityId conversationEntityId);

    Task CacheConversation(ConversationEntity conversationEntity);

    Task CacheConversationDto(ConversationDto conversationDto);

    Task InvalidateConversationCache(ConversationEntityId conversationEntityId);
}
