using Domain.Configuration;
using Domain.Dto;
using Domain.Dto.Conversation;
using Domain.Exception;
using Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize(ApplicationConstants.SessionAuthenticationScheme)]
[Route("api/v1/[controller]")]
[ApiController]
public class ConversationController(
    IConversationDtoService conversationDtoService,
    IConversationOptionService conversationOptionService) : ControllerBase
{
    [HttpGet("list")]
    public async Task<ActionResult<ServiceResponse<List<ConversationOptionDto>>>> GetConversationList()
    {
        var conversationOptionsResult = await conversationOptionService
            .GetConversationOptions(50);
        if (conversationOptionsResult.IsError)
        {
            var err = MapError(conversationOptionsResult.Error!, "Failed to get list of conversations");
            var errRes = new ServiceResponse<List<ConversationOptionDto>>(err);
            return this.Ok(errRes);
        }

        var options = conversationOptionsResult.Unwrap();
        var res = new ServiceResponse<List<ConversationOptionDto>>(options);
        return this.Ok(res);
    }

    [HttpGet("{conversationId:long}")]
    public async Task<ActionResult<ServiceResponse<ConversationDto>>> GetConversationList(long conversationId)
    {
        var conversationDtoResult = await conversationDtoService
            .GetConversationDto(conversationId);
        if (conversationDtoResult.IsError)
        {
            var err = MapError(conversationDtoResult.Error!, "Failed to get conversationDto");
            var errRes = new ServiceResponse<ConversationDto>(err);
            return this.Ok(errRes);
        }
        
        var conv = conversationDtoResult.Unwrap();
        var res = new ServiceResponse<ConversationDto>(conv);
        return this.Ok(res);
    }

    private static string MapError(Exception e, string initial)
    {
        if (e is SafeUserFeedbackException safe)
        {
            return $"{initial} -> {safe.Message}";
        }

        return initial;
    }
}
