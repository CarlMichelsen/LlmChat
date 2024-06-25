using Domain.Dto;
using Domain.Dto.SystemMessage;
using Domain.Entity.Id;

namespace Interface.Handler;

public interface ISystemMessageHandler
{
    Task<ServiceResponse<SystemMessageDto>> GetSystemMessage(SystemMessageEntityId systemMessageEntityId);

    Task<ServiceResponse<SystemMessageDto>> AddSystemMessage(EditSystemMessageDto editSystemMessage);

    Task<ServiceResponse<SystemMessageDto>> EditSystemMessage(SystemMessageEntityId systemMessageEntityId, EditSystemMessageDto editSystemMessage);

    Task<ServiceResponse> SoftDeleteSystemMessage(SystemMessageEntityId systemMessageEntityId);

    Task<ServiceResponse<List<SystemMessageDto>>> GetSystemMessageList();
}
