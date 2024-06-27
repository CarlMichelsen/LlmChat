using Domain.Abstraction;
using Domain.Dto.SystemMessage;
using Domain.Entity;
using Domain.Entity.Id;

namespace Interface.Service;

public interface ISystemMessageService
{
    Task<Result<SystemMessageEntity>> GetSystemMessage(
        SystemMessageEntityId systemMessageEntityId);
    
    Task<Result<SystemMessageEntity>> AddSystemMessage(
        EditSystemMessageDto editSystemMessage);

    Task<Result<List<SystemMessageEntity>>> GetSystemMessageList(
        int amount);

    Task<Result<SystemMessageEntity>> EditSystemMessage(
        SystemMessageEntityId systemMessageEntityId,
        EditSystemMessageDto editSystemMessage);

    Task<Result<int>> SoftDeleteSystemMessage(
        SystemMessageEntityId systemMessageEntityId);
}
