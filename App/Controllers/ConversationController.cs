using Domain.Configuration;
using Domain.Dto;
using Domain.Dto.Conversation;
using Domain.Entity.Id;
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
    [HttpGet("{conversationId}")]
    public async Task<ActionResult<ServiceResponse<ConversationDto>>> GetConversation(Guid conversationId)
    {
        var serviceResponse = await conversationHandler.GetConversation(new ConversationEntityId(conversationId));
        return this.Ok(serviceResponse);
    }

    [HttpGet("list")]
    public async Task<ActionResult<ServiceResponse<List<ConversationOptionDto>>>> GetConversationList()
    {
        var serviceResponse = await conversationHandler.GetConversationList();
        return this.Ok(serviceResponse);
    }
}
