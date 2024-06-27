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
    public async Task<ServiceResponse<SystemMessageDto>> AddSystemMessage(EditSystemMessageDto editSystemMessage)
    {
        var addedResult = await systemMessageService
            .AddSystemMessage(editSystemMessage);
        
        if (addedResult.IsError)
        {
            return ServiceResponse<SystemMessageDto>
                .CreateErrorResponse("Failed to add system message", addedResult.Error!);
        }

        var mapResult = SystemMessageDtoMapper.Map(addedResult.Unwrap());
        if (mapResult.IsError)
        {
            return ServiceResponse<SystemMessageDto>
                .CreateErrorResponse("Failed to map system message", mapResult.Error!);
        }
        
        return new ServiceResponse<SystemMessageDto>(mapResult.Unwrap());
    }

    public async Task<ServiceResponse<SystemMessageDto>> EditSystemMessage(SystemMessageEntityId systemMessageEntityId, EditSystemMessageDto editSystemMessage)
    {
        var contentEditResult = await systemMessageService
            .EditSystemMessage(systemMessageEntityId, editSystemMessage);
        
        if (contentEditResult.IsError)
        {
            return ServiceResponse<SystemMessageDto>
                .CreateErrorResponse("Failed to edit system message content", contentEditResult.Error!);
        }

        var mapResult = SystemMessageDtoMapper.Map(contentEditResult.Unwrap());
        if (mapResult.IsError)
        {
            return ServiceResponse<SystemMessageDto>
                .CreateErrorResponse("Failed to map system message", mapResult.Error!);
        }
        
        return new ServiceResponse<SystemMessageDto>(mapResult.Unwrap());
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
