using Interface.Service;
using LargeLanguageModelClient;
using LargeLanguageModelClient.Dto.Model;
using Microsoft.Extensions.Caching.Distributed;

namespace Implementation.Service;

public class ModelService(
    ILargeLanguageModelClient largeLanguageModelClient,
    ICacheService cacheService) : IModelService
{
    public async Task<LlmModelDto?> GetModel(Guid modelIdentifier)
    {
        var cacheKey = this.GenerateCacheKey(modelIdentifier);
        var cachedModel = await cacheService.GetJson<LlmModelDto>(cacheKey);
        if (cachedModel is not null)
        {
            return cachedModel;
        }

        var model = await largeLanguageModelClient.GetModel(modelIdentifier);
        if (model is null)
        {
            return null;
        }

        if (!model.Ok)
        {
            return null;
        }

        await cacheService.SetJson(cacheKey, model.Data, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
        });
        return model.Data;
    }

    private string GenerateCacheKey(Guid modelIdentifier)
    {
        return $"model-identifier-{modelIdentifier}";
    }
}
