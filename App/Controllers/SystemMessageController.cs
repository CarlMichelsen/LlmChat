using Domain.Configuration;
using Domain.Dto;
using Domain.Dto.SystemMessage;
using Domain.Entity.Id;
using Interface.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize(ApplicationConstants.SessionAuthenticationScheme)]
[Route("api/v1/[controller]")]
[ApiController]
public class SystemMessageController(
    ISystemMessageHandler systemMessageHandler) : ControllerBase
{
    [HttpGet("{systemMessageId}")]
    public async Task<ActionResult<ServiceResponse<SystemMessageDto>>> GetSystemMessage([FromRoute] Guid systemMessageId)
    {
        var result = await systemMessageHandler
            .GetSystemMessage(new SystemMessageEntityId(systemMessageId));
        return this.Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<SystemMessageDto>>> AddSystemMessage([FromBody] EditSystemMessageDto addSystemMessage)
    {
        var result = await systemMessageHandler
            .AddSystemMessage(addSystemMessage);
        return this.Ok(result);
    }
    
    [HttpPut("{systemMessageId}")]
    public async Task<ActionResult<ServiceResponse>> EditSystemMessageName([FromRoute] Guid systemMessageId, [FromBody] EditSystemMessageDto addSystemMessage)
    {
        var result = await systemMessageHandler
            .EditSystemMessage(new SystemMessageEntityId(systemMessageId), addSystemMessage);
        return this.Ok(result);
    }

    [HttpDelete("{systemMessageId}")]
    public async Task<ActionResult<ServiceResponse>> SoftDeleteSystemMessage([FromRoute] Guid systemMessageId)
    {
        var result = await systemMessageHandler
            .SoftDeleteSystemMessage(new SystemMessageEntityId(systemMessageId));
        return this.Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<SystemMessageDto>>>> GetSystemMessageList()
    {
        var result = await systemMessageHandler
            .GetSystemMessageList();
        return this.Ok(result);
    }
}
