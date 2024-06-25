using Domain.Dto;
using Domain.Dto.SystemMessage;
using Domain.Entity.Id;

namespace Interface.Handler;

public interface ISystemMessageHandler
{
    Task<ServiceResponse<SystemMessageDto>> GetSystemMessage(SystemMessageEntityId systemMessageEntityId);

    Task<ServiceResponse> EditSystemMessageContent(SystemMessageEntityId systemMessageEntityId, string content);
    
    Task<ServiceResponse> EditSystemMessageName(SystemMessageEntityId systemMessageEntityId, string name);

    Task<ServiceResponse> SoftDeleteSystemMessage(SystemMessageEntityId systemMessageEntityId);

    Task<ServiceResponse<List<SystemMessageDto>>> GetSystemMessageList();
}
