using Domain.Dto;
using Domain.Dto.SystemMessage;
using Domain.Entity.Id;
using Implementation.Map;
using Interface.Handler;
using Interface.Service;

namespace Implementation.Handler;

public class SystemMessageHandler(
    ISystemMessageService systemMessageService) : ISystemMessageHandler
{
    public async Task<ServiceResponse> EditSystemMessageContent(SystemMessageEntityId systemMessageEntityId, string content)
    {
        var contentEditResult = await systemMessageService
            .EditSystemMessageContent(systemMessageEntityId, content);
        
        if (contentEditResult.IsError)
        {
            return ServiceResponse
                .CreateErrorResponse("Failed to edit system message content", contentEditResult.Error!);
        }
        
        return new ServiceResponse();
    }

    public async Task<ServiceResponse> EditSystemMessageName(SystemMessageEntityId systemMessageEntityId, string name)
    {
        var contentEditResult = await systemMessageService
            .EditSystemMessageName(systemMessageEntityId, name);
        
        if (contentEditResult.IsError)
        {
            return ServiceResponse
                .CreateErrorResponse("Failed to edit system message name", contentEditResult.Error!);
        }
        
        return new ServiceResponse();
    }

    public async Task<ServiceResponse<SystemMessageDto>> GetSystemMessage(SystemMessageEntityId systemMessageEntityId)
    {
        var systemMessageResult = await systemMessageService
            .GetSystemMessage(systemMessageEntityId);
        
        if (systemMessageResult.IsError)
        {
            return ServiceResponse<SystemMessageDto>
                .CreateErrorResponse("Failed to get system message", systemMessageResult.Error!);
        }

        var mapResult = SystemMessageDtoMapper.Map(systemMessageResult.Unwrap());
        if (mapResult.IsError)
        {
            return ServiceResponse<SystemMessageDto>
                .CreateErrorResponse("Failed to map system message", mapResult.Error!);
        }
        
        return new ServiceResponse<SystemMessageDto>(mapResult.Unwrap());
    }

    public async Task<ServiceResponse<List<SystemMessageDto>>> GetSystemMessageList()
    {
        var systemMessageListResult = await systemMessageService
            .GetSystemMessageList(50);
        
        if (systemMessageListResult.IsError)
        {
            return ServiceResponse<List<SystemMessageDto>>
                .CreateErrorResponse("Failed to get system message list", systemMessageListResult.Error!);
        }

        var mapResults = systemMessageListResult.Unwrap()
            .Select(SystemMessageDtoMapper.Map)
            .ToList();
        
        var err = mapResults.FirstOrDefault(mr => mr.IsError);
        if (err is not null)
        {
            return ServiceResponse<List<SystemMessageDto>>
                .CreateErrorResponse("Failed to map system messages", err.Error!);
        }
        
        return new ServiceResponse<List<SystemMessageDto>>(
            mapResults.Select(mr => mr.Unwrap()).ToList());
    }

    public async Task<ServiceResponse> SoftDeleteSystemMessage(SystemMessageEntityId systemMessageEntityId)
    {
        var contentDeleteResult = await systemMessageService
            .SoftDeleteSystemMessage(systemMessageEntityId);
        
        if (contentDeleteResult.IsError)
        {
            return ServiceResponse
                .CreateErrorResponse("Failed to delete system message", contentDeleteResult.Error!);
        }
        
        return new ServiceResponse();
    }
}
