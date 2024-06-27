using Domain.Abstraction;
using Domain.Dto.SystemMessage;
using Domain.Entity;
using Domain.Entity.Id;
using Domain.Exception;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Service;

public class SystemMessageService(
    ISessionService sessionService,
    ISystemMessageRepository systemMessageRepository) : ISystemMessageService
{
    private readonly ProfileEntityId profileId = new(sessionService.UserProfileId);

    public async Task<Result<SystemMessageEntity>> AddSystemMessage(
        EditSystemMessageDto editSystemMessage)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(editSystemMessage.Name))
            {
                return new SafeUserFeedbackException("Name must be defined to create system message");
            }

            if (string.IsNullOrWhiteSpace(editSystemMessage.Content))
            {
                return new SafeUserFeedbackException("Content must be defined to create system message");
            }

            var addedMessage = await systemMessageRepository.AddSystemMessage(
                this.profileId,
                editSystemMessage.Name,
                editSystemMessage.Content);

            return addedMessage;
        }
        catch (System.Exception e)
        {
            return e;
        }
    }

    public async Task<Result<SystemMessageEntity>> EditSystemMessage(
        SystemMessageEntityId systemMessageEntityId,
        EditSystemMessageDto editSystemMessage)
    {
        try
        {
            var editedMessage = await systemMessageRepository.EditSystemMessage(
                this.profileId,
                systemMessageEntityId,
                editSystemMessage.Name,
                editSystemMessage.Content);
            
            return editedMessage;
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
