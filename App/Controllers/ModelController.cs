using Domain.Configuration;
using Domain.Dto;
using Domain.Dto.LargeLanguageModel;
using Interface.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize(ApplicationConstants.SessionAuthenticationScheme)]
[Route("api/v1/[controller]")]
[ApiController]
public class ModelController(
    IModelHandler modelHandler) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<ServiceResponse<List<LargeLanguageModelDto>>>> GetAllModels()
    {
        var modelDtoServiceResponse = await modelHandler.GetAvailableModels();
        return this.Ok(modelDtoServiceResponse);
    }
}
