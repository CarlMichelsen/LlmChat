using System.Text.Json.Serialization;

namespace Domain.Dto.LargeLanguageModel;

public class LargeLanguageModelDto
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("providerName")]
    public required string ProviderName { get; init; }

    [JsonPropertyName("price")]
    public required LargeLanguageModelPriceDto Price { get; init; }

    [JsonPropertyName("modelDisplayName")]
    public required string ModelDisplayName { get; init; }

    [JsonPropertyName("modelDescription")]
    public required string ModelDescription { get; init; }

    [JsonPropertyName("modelIdentifierName")]
    public required string ModelIdentifierName { get; init; }

    [JsonPropertyName("maxResponseTokenCount")]
    public required long MaxResponseTokenCount { get; init; }

    [JsonPropertyName("maxContextTokenCount")]
    public required long MaxContextTokenCount { get; init; }

    [JsonPropertyName("imageSupport")]
    public required bool ImageSupport { get; init; }

    [JsonPropertyName("videoSupport")]
    public required bool VideoSupport { get; init; }

    [JsonPropertyName("jsonResponseOptimized")]
    public required bool JsonResponseOptimized { get; init; }
}