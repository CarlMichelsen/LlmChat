using Domain.Abstraction;
using Domain.Entity;
using Domain.Entity.Id;

namespace Interface.Service;

public interface ISystemMessageService
{
    Task<Result<SystemMessageEntity>> GetSystemMessage(SystemMessageEntityId systemMessageEntityId);

    Task<Result<List<SystemMessageEntity>>> GetSystemMessageList(int amount);

    Task<Result<int>> EditSystemMessageContent(SystemMessageEntityId systemMessageEntityId, string content);
    
    Task<Result<int>> EditSystemMessageName(SystemMessageEntityId systemMessageEntityId, string name);

    Task<Result<int>> SoftDeleteSystemMessage(SystemMessageEntityId systemMessageEntityId);
}
