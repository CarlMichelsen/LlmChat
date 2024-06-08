using System.Text.Json.Serialization;

namespace Domain.Dto.LargeLanguageModel;

public class LargeLanguageModelPriceDto
{
    [JsonPropertyName("millionInputTokenPrice")]
    public required int MillionInputTokenCentPrice { get; init; }

    [JsonPropertyName("millionOutputTokenPrice")]
    public required int MillionOutputTokenCentPrice { get; init; }
}
