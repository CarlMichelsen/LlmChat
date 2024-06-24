using Domain.Abstraction;
using Domain.Dto.Conversation;
using Domain.Entity;
using Domain.Entity.Id;
using Interface.Repository;
using Interface.Service;
using Microsoft.Extensions.Caching.Distributed;

namespace Implementation.Service;

public class ConversationOptionService(
    IConversationReadRepository conversationReadRepository,
    ICacheService cacheService,
    ISessionService sessionService) : IConversationOptionService
{
    public async Task<Result<List<ConversationOptionDto>>> GetConversationOptions(int amount)
    {
        var creatorIdentifier = sessionService.UserProfileId;
        var cacheKey = CacheKeys.GenerateConversationOptionsCacheKey(creatorIdentifier);
        var cachedConversationOptions = await cacheService.GetJson<List<ConversationOptionDto>>(cacheKey);
        if (cachedConversationOptions is not null)
        {
            return cachedConversationOptions;
        }
        
        var conversationsResult = await conversationReadRepository
            .GetShallowConversations(new ProfileEntityId(creatorIdentifier), amount);
        if (conversationsResult.IsError)
        {
            return conversationsResult.Error!;
        }

        var optionDtos = conversationsResult
            .Unwrap()
            .Select(Map)
            .ToList();
        await cacheService.SetJson(cacheKey, optionDtos, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(15),
            });

        return optionDtos;
    }

    private static ConversationOptionDto Map(ConversationEntity conversationEntity)
    {
        return new ConversationOptionDto
        {
            Id = conversationEntity.Id.Value,
            Summary = conversationEntity.Summary,
            LastAppendedEpoch = new DateTimeOffset(conversationEntity.LastAppendedUtc).ToUnixTimeSeconds(),
            CreatedEpoch = new DateTimeOffset(conversationEntity.CreatedUtc).ToUnixTimeSeconds(),
        };
    }
}
