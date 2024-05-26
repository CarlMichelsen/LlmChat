using Domain.Configuration;
using Domain.Dto;
using Domain.Session;
using Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize(ApplicationConstants.SessionAuthenticationScheme)]
[Route("api/v1/[controller]")]
[ApiController]
public class SessionController(
    ISessionService sessionService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<UserData>>> GetSessionData()
    {
        var sessionData = await sessionService.GetSessionData();
        if (sessionData.IsError)
        {
            var notLoggedInRes = new ServiceResponse<UserData>("User not logged in");
            return this.Ok(notLoggedInRes);
        }

        var loggedInRes = new ServiceResponse<UserData>(sessionData.Unwrap().User);
        return this.Ok(loggedInRes);
    }

    [HttpDelete]
    public async Task<ActionResult<ServiceResponse<bool>>> RemoveSessionData()
    {
        var sessionData = await sessionService.RemoveSessionData();
        if (sessionData.IsError)
        {
            var failureRes = new ServiceResponse<UserData>("Failed to logout user");
            return this.Ok(failureRes);
        }

        return this.Ok(new ServiceResponse<bool>(true));
    }
}
