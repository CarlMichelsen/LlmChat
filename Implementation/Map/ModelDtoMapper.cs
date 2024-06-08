using Domain.Dto.LargeLanguageModel;
using LargeLanguageModelClient.Dto.Model;

namespace Implementation.Map;

public static class ModelDtoMapper
{
    public static LargeLanguageModelDto Map(LlmModelDto model)
    {
        return new LargeLanguageModelDto
        {
            Id = model.Id,
            ProviderName = model.ProviderName,
            Price = Map(model.Price),
            ModelDisplayName = model.ModelDisplayName,
            ModelDescription = model.ModelDescription,
            ModelIdentifierName = model.ModelIdentifierName,
            MaxResponseTokenCount = model.MaxTokenCount,
            MaxContextTokenCount = model.ContextTokenCount,
            ImageSupport = model.ImageSupport,
            VideoSupport = model.VideoSupport,
            JsonResponseOptimized = model.JsonResponseOptimized,
        };
    }

    public static LargeLanguageModelPriceDto Map(LlmPriceDto price)
    {
        return new LargeLanguageModelPriceDto
        {
            MillionInputTokenCentPrice = price.MillionInputTokenPrice,
            MillionOutputTokenCentPrice = price.MillionOutputTokenPrice,
        };
    }
}
