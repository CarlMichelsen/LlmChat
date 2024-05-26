using System.Text.Json.Serialization;

namespace Domain.Dto.Chat.Stream;

public record ContentConcludedDto(
    [property: JsonPropertyName("inputTokens")] long InputTokens,
    [property: JsonPropertyName("outputTokens")] long OutputTokens);