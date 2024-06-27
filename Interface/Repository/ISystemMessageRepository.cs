using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;

namespace Interface.Repository;

public interface ISystemMessageRepository
{
    Task<Result<SystemMessageEntity>> GetSystemMessage(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId);

    Task<Result<List<SystemMessageEntity>>> GetSystemMessageList(
        ProfileEntityId creatorId,
        int amount);

    Task<Result<SystemMessageEntity>> AddSystemMessage(
        ProfileEntityId creatorId,
        string name,
        string content);

    Task<Result<SystemMessageEntity>> EditSystemMessage(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId,
        string? name,
        string? content);

    Task<Result<int>> SoftDeleteSystemMessage(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId);
}
