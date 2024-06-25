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

    Task<Result<int>> EditSystemMessageContent(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId,
        string content);
    
    Task<Result<int>> EditSystemMessageName(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId,
        string name);

    Task<Result<int>> SoftDeleteSystemMessage(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId);
}
