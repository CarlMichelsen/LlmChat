using Domain.Abstraction;
using Domain.Configuration;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using Implementation.Database;
using Interface.Repository;
using Microsoft.Extensions.Options;

namespace Implementation.Repository;

public class ProfileRepository(
    IOptions<ProfileDefaultOptions> profileDefaultOptions,
    ApplicationContext applicationContext) : IProfileRepository
{
    public async Task<Result<ProfileEntity>> GetAndOrCreateProfile(ProfileEntityId profileIdentifier)
    {
        try
        {
            var profile = await applicationContext.Profiles.FindAsync(profileIdentifier);
            if (profile is null)
            {
                profile = this.Create(profileIdentifier);
                applicationContext.Profiles.Add(profile);
            }

            return profile;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<string>> SetDefaultSystemMessage(ProfileEntityId profileIdentifier, string systemMessage)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(systemMessage))
            {
                return new SafeUserFeedbackException("Invalid system message");
            }

            var profile = await applicationContext.Profiles.FindAsync(profileIdentifier);
            if (profile is null)
            {
                return new SafeUserFeedbackException("Failed to find profile to set default system message");
            }

            profile.DefaultSystemMessage = systemMessage;
            return profile.DefaultSystemMessage;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    private ProfileEntity Create(ProfileEntityId profileIdentifier)
    {
        return new ProfileEntity
        {
            Id = profileIdentifier,
            DefaultSystemMessage = profileDefaultOptions.Value.SystemMessage,
            SelectedModel = profileDefaultOptions.Value.ModelIdentifier,
        };
    }
}
