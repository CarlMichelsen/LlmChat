using Domain.Dto;
using Domain.Entity.Id;
using Interface.Handler;
using Interface.Repository;
using Interface.Service;
using Microsoft.Extensions.Caching.Distributed;

namespace Implementation.Handler;

public class ProfileHandler(
    ICacheService cacheService,
    ISessionService sessionService,
    IProfileRepository profileRepository) : IProfileHandler
{
    public async Task<ServiceResponse<string>> GetDefaultSystemMessage()
    {
        var cacheKey = this.GetCacheKey();
        var cachedMessage = await cacheService.Get(cacheKey);
        if (cachedMessage is not null)
        {
            return new ServiceResponse<string>(cachedMessage);
        }

        var profileResult = await profileRepository
            .GetAndOrCreateProfile(new ProfileEntityId(sessionService.UserProfileId));
        
        if (profileResult.IsError)
        {
            return ServiceResponse<string>.CreateErrorResponse(
                "Failed to get default system message for user",
                profileResult.Error!);
        }

        var message = profileResult.Unwrap().DefaultSystemMessage;
        await cacheService.Set(cacheKey, message, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(15),
        });

        return new ServiceResponse<string>(message);
    }

    public async Task<ServiceResponse<string>> SetDefaultSystemMessage(string systemMessage)
    {
        var newSystemMessageResult = await profileRepository.SetDefaultSystemMessage(
            new ProfileEntityId(sessionService.UserProfileId),
            systemMessage);

        if (newSystemMessageResult.IsError)
        {
            return ServiceResponse<string>.CreateErrorResponse(
                "Failed to set default system message on profile",
                newSystemMessageResult.Error!);
        }

        var cacheKey = this.GetCacheKey();
        await cacheService.Remove(cacheKey);

        return new ServiceResponse<string>(newSystemMessageResult.Unwrap());
    }

    private string GetCacheKey()
        => CacheKeys.GenerateDefaultSystemMessageCacheKey(sessionService.UserProfileId);
}
