using Domain.Configuration;
using Domain.Dto.Chat;
using Interface.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize(ApplicationConstants.SessionAuthenticationScheme)]
[Route("api/v1/[controller]")]
[ApiController]
public class ChatController(
    ILogger<ChatController> logger,
    IMessageHandler messageHandler) : ControllerBase
{
    [HttpPost]
    public async Task SendMessage([FromBody] NewMessageDto newUserMessageDto)
    {
        logger.LogInformation("SendMessage\n{Message}", newUserMessageDto.Content.First().Content);
        await messageHandler.SendMessage(newUserMessageDto, this.HttpContext.RequestAborted);
    }
}
