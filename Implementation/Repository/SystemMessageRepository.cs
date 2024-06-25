using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using Implementation.Database;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Repository;

public class SystemMessageRepository(
    ApplicationContext applicationContext) : ISystemMessageRepository
{
    public async Task<Result<int>> EditSystemMessageContent(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId,
        string content)
    {
        try
        {
            var changedRows = await applicationContext.SystemMessages
                .Include(sm => sm.Creator)
                .Where(sm => sm.Creator.Id == creatorId && sm.Id == systemMessageEntityId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(sm => sm.Content, content)
                    .SetProperty(sm => sm.LastAppendedUtc, DateTime.UtcNow));
            
            if (changedRows == 0)
            {
                return new SafeUserFeedbackException("Did not update anything");
            }

            return changedRows;
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<int>> EditSystemMessageName(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId,
        string name)
    {
        try
        {
            var changedRows = await applicationContext.SystemMessages
                .Include(sm => sm.Creator)
                .Where(sm => sm.Creator.Id == creatorId && sm.Id == systemMessageEntityId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(sm => sm.Name, name)
                    .SetProperty(sm => sm.LastAppendedUtc, DateTime.UtcNow));
            
            if (changedRows == 0)
            {
                return new SafeUserFeedbackException("Did not update anything");
            }

            return changedRows;
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<SystemMessageEntity>> GetSystemMessage(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId)
    {
        try
        {
            var systemMessage = await applicationContext.SystemMessages
                .Include(sm => sm.Creator)
                .FirstOrDefaultAsync(sm => sm.Creator.Id == creatorId && sm.Id == systemMessageEntityId);
            
            if (systemMessage is null)
            {
                return new SafeUserFeedbackException("Did not find system message");
            }
            
            return systemMessage;
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<List<SystemMessageEntity>>> GetSystemMessageList(
        ProfileEntityId creatorId,
        int amount)
    {
        try
        {
            return await applicationContext.SystemMessages
                .Include(sm => sm.Creator)
                .Where(sm => sm.Creator.Id == creatorId)
                .OrderByDescending(sm => sm.LastAppendedUtc)
                .Take(amount)
                .ToListAsync();
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<int>> SoftDeleteSystemMessage(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId)
    {
        try
        {
            var systemMessage = await applicationContext.SystemMessages
                .Include(sm => sm.Creator)
                .FirstOrDefaultAsync(sm => sm.Creator.Id == creatorId && sm.Id == systemMessageEntityId);
            
            if (systemMessage is null)
            {
                return new SafeUserFeedbackException("Did not find system message");
            }

            applicationContext.SystemMessages.Remove(systemMessage);
            return await applicationContext.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            return e;
        }
    }
}
