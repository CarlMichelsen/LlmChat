using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;

namespace Interface.Repository;

public interface IProfileRepository
{
    public Task<Result<ProfileEntity>> GetAndOrCreateProfile(
        ProfileEntityId profileIdentifier);

    public Task<Result<string>> SetDefaultSystemMessage(
        ProfileEntityId profileIdentifier,
        string systemMessage);
}
