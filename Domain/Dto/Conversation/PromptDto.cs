using System.Text.Json.Serialization;

namespace Domain.Dto.Conversation;

public class PromptDto
{
    [JsonPropertyName("modelName")]
    public required string ModelName { get; init; }

    [JsonPropertyName("modelId")]
    public required Guid ModelId { get; init; }

    [JsonPropertyName("providerPromptIdentifier")]
    public required string ProviderPromptIdentifier { get; init; }

    [JsonPropertyName("inputTokens")]
    public required int InputTokens { get; init; }

    [JsonPropertyName("outputTokens")]
    public required int OutputTokens { get; init; }

    [JsonPropertyName("stopReason")]
    public required string StopReason { get; init; }
}
