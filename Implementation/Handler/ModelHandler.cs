using Domain.Dto;
using Domain.Dto.LargeLanguageModel;
using Implementation.Map;
using Interface.Handler;
using Interface.Service;

namespace Implementation.Handler;

public class ModelHandler(
    IModelService modelService) : IModelHandler
{
    public async Task<ServiceResponse<List<LargeLanguageModelDto>>> GetAvailableModels()
    {
        var models = await modelService.GetAllModels();
        if (models is null)
        {
            return new ServiceResponse<List<LargeLanguageModelDto>>("No models found");
        }

        var mapped = models
            .Where(m => m.Available)
            .Select(ModelDtoMapper.Map)
            .ToList();
        
        return new ServiceResponse<List<LargeLanguageModelDto>>(mapped);
    }
}
