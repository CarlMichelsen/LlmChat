using Domain.Configuration;
using Domain.Dto;
using Domain.Session;
using Interface.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SessionController(
    ISessionHandler sessionHandler) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<UserData>>> GetSessionData()
    {
        var sessionData = await sessionHandler.GetSessionData();
        return this.Ok(sessionData);
    }

    [HttpDelete]
    [Authorize(ApplicationConstants.SessionAuthenticationScheme)]
    public async Task<ActionResult<ServiceResponse<bool>>> RemoveSessionData()
    {
        var removed = await sessionHandler.RemoveSessionData();
        return this.Ok(removed);
    }
}
