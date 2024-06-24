using Domain.Dto.Conversation;
using Domain.Entity;
using Domain.Entity.Id;
using Implementation.Map;
using Interface.Service;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Implementation.Service;

public class ConversationDtoCacheService(
    ILogger<ConversationDtoCacheService> logger,
    ISessionService sessionService,
    ICacheService cacheService) : IConversationDtoCacheService
{
    public async Task CacheConversation(ConversationEntity conversationEntity)
    {
        var dtoResult = ConversationDtoMapper.Map(conversationEntity);
        if (dtoResult.IsError)
        {
            logger.LogCritical(dtoResult.Error!, $"Failed to map conversation<${conversationEntity.Id.Value}> to dto.");
            return;
        }

        await this.CacheConversationDto(dtoResult.Unwrap());
    }

    public async Task CacheConversationDto(ConversationDto conversationDto)
    {
        var key = CacheKeys.GenerateConversationCacheKey(sessionService.UserProfileId, conversationDto.Id);
        await cacheService.SetJson(key, conversationDto, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(15),
            });
    }

    public async Task<ConversationDto?> GetConversationDto(ConversationEntityId conversationEntityId)
    {
        var key = CacheKeys.GenerateConversationCacheKey(sessionService.UserProfileId, conversationEntityId.Value);
        return await cacheService.GetJson<ConversationDto>(key);
    }

    public async Task InvalidateConversationCache(ConversationEntityId conversationEntityId)
    {
        var key = CacheKeys.GenerateConversationCacheKey(sessionService.UserProfileId, conversationEntityId.Value);
        await cacheService.Remove(key);
    }
}
