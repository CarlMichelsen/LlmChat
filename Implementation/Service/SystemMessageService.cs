using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Service;

public class SystemMessageService(
    ISessionService sessionService,
    ISystemMessageRepository systemMessageRepository) : ISystemMessageService
{
    private readonly ProfileEntityId profileId = new(sessionService.UserProfileId);

    public async Task<Result<int>> EditSystemMessageContent(
        SystemMessageEntityId systemMessageEntityId,
        string content)
    {
        try
        {
            var editSystemResult = await systemMessageRepository
                .EditSystemMessageContent(this.profileId, systemMessageEntityId, content);
            
            if (editSystemResult.IsError)
            {
                return editSystemResult.Error!;
            }

            var cacheKeys = CacheKeys.GenerateSystemMessagesCacheKey(sessionService.UserProfileId);
            return editSystemResult.Unwrap();
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<int>> EditSystemMessageName(
        SystemMessageEntityId systemMessageEntityId,
        string name)
    {
        try
        {
            var editSystemResult = await systemMessageRepository
                .EditSystemMessageName(this.profileId, systemMessageEntityId, name);
            
            if (editSystemResult.IsError)
            {
                return editSystemResult.Error!;
            }

            var cacheKey = CacheKeys
                .GenerateSystemMessagesCacheKey(sessionService.UserProfileId);
            return editSystemResult.Unwrap();
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<SystemMessageEntity>> GetSystemMessage(
        SystemMessageEntityId systemMessageEntityId)
    {
        try
        {
            var systemMessageResult = await systemMessageRepository
                .GetSystemMessage(this.profileId, systemMessageEntityId);
            
            if (systemMessageResult.IsError)
            {
                return systemMessageResult.Error!;
            }

            return systemMessageResult.Unwrap();
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<List<SystemMessageEntity>>> GetSystemMessageList(int amount)
    {
        try
        {
            var systemMessageListResult = await systemMessageRepository
                .GetSystemMessageList(this.profileId, amount);
            
            if (systemMessageListResult.IsError)
            {
                return systemMessageListResult.Error!;
            }

            return systemMessageListResult.Unwrap();
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<int>> SoftDeleteSystemMessage(
        SystemMessageEntityId systemMessageEntityId)
    {
        try
        {
            var deletedResult = await systemMessageRepository
                .SoftDeleteSystemMessage(this.profileId, systemMessageEntityId);
            
            if (deletedResult.IsError)
            {
                return deletedResult.Error!;
            }

            return deletedResult;
        }
        catch (System.Exception e)
        {
            return e;
        }
    }
}
