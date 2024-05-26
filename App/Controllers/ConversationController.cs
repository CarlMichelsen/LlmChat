using Domain.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize(ApplicationConstants.SessionAuthenticationScheme)]
[Route("api/v1/[controller]")]
[ApiController]
public class ConversationController : ControllerBase
{
    [HttpGet("list")]
    public async Task<ActionResult> GetConversationList()
    {
        await Task.Delay(200);
        return this.Ok();
    }
}
