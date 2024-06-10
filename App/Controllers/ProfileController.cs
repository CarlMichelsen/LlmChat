using Domain.Configuration;
using Domain.Dto;
using Interface.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize(ApplicationConstants.SessionAuthenticationScheme)]
[Route("api/v1/[controller]")]
[ApiController]
public class ProfileController(IProfileHandler profileHandler) : ControllerBase
{
    [HttpPost("system")]
    public async Task<ActionResult<ServiceResponse<string>>> SetDefaultSystemMessage([FromBody] string defaultSystemMessage)
    {
        var defaultSystemMessageResponse = await profileHandler
            .SetDefaultSystemMessage(defaultSystemMessage);
        
        return this.Ok(defaultSystemMessageResponse);
    }

    [HttpGet("system")]
    public async Task<ActionResult<ServiceResponse<string>>> GetDefaultSystemMessage()
    {
        var defaultSystemMessageResponse = await profileHandler
            .GetDefaultSystemMessage();
        
        return this.Ok(defaultSystemMessageResponse);
    }
}
