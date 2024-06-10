using Domain.Dto;
using Domain.Entity.Id;
using Interface.Handler;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Handler;

public class ProfileHandler(
    ISessionService sessionService,
    IProfileRepository profileRepository) : IProfileHandler
{
    public async Task<ServiceResponse<string>> GetDefaultSystemMessage()
    {
        var profileResult = await profileRepository
            .GetAndOrCreateProfile(new ProfileEntityId(sessionService.UserProfileId));
        
        if (profileResult.IsError)
        {
            return ServiceResponse<string>.CreateErrorResponse(
                "Failed to get default system message for user",
                profileResult.Error!);
        }

        return new ServiceResponse<string>(profileResult.Unwrap().DefaultSystemMessage);
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

        return new ServiceResponse<string>(newSystemMessageResult.Unwrap());
    }
}
