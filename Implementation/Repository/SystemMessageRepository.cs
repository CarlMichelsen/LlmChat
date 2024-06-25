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
    public async Task<Result<SystemMessageEntity>> AddSystemMessage(
        ProfileEntityId creatorId,
        string name,
        string content)
    {
        try
        {
            var profile = await applicationContext.Profiles
                .FirstOrDefaultAsync(p => p.Id == creatorId);

            if (profile is null)
            {
                return new SafeUserFeedbackException("Did not find profile to add system message to");
            }

            var newSystemMessage = new SystemMessageEntity
            {
                Id = new SystemMessageEntityId(Guid.NewGuid()),
                Creator = profile,
                Name = name,
                Content = content,
                LastAppendedUtc = DateTime.UtcNow,
                CreatedUtc = DateTime.UtcNow,
            };

            applicationContext.SystemMessages.Add(newSystemMessage);

            await applicationContext.SaveChangesAsync();
            applicationContext.Entry(newSystemMessage).State = EntityState.Detached;
            return newSystemMessage;
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<SystemMessageEntity>> EditSystemMessage(
        ProfileEntityId creatorId,
        SystemMessageEntityId systemMessageEntityId,
        string? name,
        string? content)
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

            if (!string.IsNullOrWhiteSpace(name))
            {
                systemMessage.Name = name;
            }

            if (!string.IsNullOrWhiteSpace(content))
            {
                systemMessage.Content = content;
            }

            await applicationContext.SaveChangesAsync();
            applicationContext.Entry(systemMessage).State = EntityState.Detached;
            return systemMessage;
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
            
            applicationContext.Entry(systemMessage).State = EntityState.Detached;
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
            var list = await applicationContext.SystemMessages
                .Include(sm => sm.Creator)
                .Where(sm => sm.Creator.Id == creatorId)
                .OrderByDescending(sm => sm.LastAppendedUtc)
                .Take(amount)
                .ToListAsync();
            
            list.ForEach(sm => applicationContext.Entry(sm).State = EntityState.Detached);
            return list;
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
