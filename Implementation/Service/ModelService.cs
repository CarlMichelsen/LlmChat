using Interface.Service;
using LargeLanguageModelClient;
using LargeLanguageModelClient.Dto.Model;
using Microsoft.Extensions.Caching.Distributed;

namespace Implementation.Service;

public class ModelService(
    ILargeLanguageModelClient largeLanguageModelClient,
    ICacheService cacheService) : IModelService
{
    public const string ModelsCacheKey = "large-language-model-list";
    private List<LlmModelDto>? llmModelDtos = null;

    public async Task<LlmModelDto?> GetModel(Guid modelIdentifier)
    {
        var models = await this.GetModelList();
        if (models is null)
        {
            return default;
        }

        return models.FirstOrDefault(m => m.Id == modelIdentifier);
    }

    public async Task<List<LlmModelDto>?> GetAllModels()
    {
        var models = await this.GetModelList();
        if (models is null)
        {
            return default;
        }

        return models;
    }

    private async Task<List<LlmModelDto>?> GetModelList()
    {
        if (this.llmModelDtos is not null)
        {
            return this.llmModelDtos;
        }

        var cachedModels = await cacheService.GetJson<List<LlmModelDto>>(ModelsCacheKey);
        if (cachedModels is not null)
        {
            this.llmModelDtos = cachedModels;
            return this.llmModelDtos;
        }

        var response = await largeLanguageModelClient.GetAllModels();
        if (response.Ok)
        {
            this.llmModelDtos = response.Data!;
            await cacheService.SetJson(ModelsCacheKey, this.llmModelDtos, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            });
            
            return this.llmModelDtos;
        }

        return default;
    }
}
