using Domain.Configuration;
using Domain.Dto;
using Domain.Dto.Conversation;
using Interface.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize(ApplicationConstants.SessionAuthenticationScheme)]
[Route("api/v1/[controller]")]
[ApiController]
public class ConversationController(
    IConversationHandler conversationHandler) : ControllerBase
{
    [HttpGet("{conversationId:long}")]
    public async Task<ActionResult<ServiceResponse<ConversationDto>>> GetConversation(long conversationId)
    {
        var serviceResponse = await conversationHandler.GetConversation(conversationId);
        return this.Ok(serviceResponse);
    }

    [HttpGet("list")]
    public async Task<ActionResult<ServiceResponse<List<ConversationOptionDto>>>> GetConversationList()
    {
        var serviceResponse = await conversationHandler.GetConversationList();
        return this.Ok(serviceResponse);
    }
}
