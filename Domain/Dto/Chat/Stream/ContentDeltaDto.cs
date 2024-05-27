using System.Text.Json.Serialization;

namespace Domain.Dto.Chat.Stream;

public record ContentDeltaDto(
    [property: JsonPropertyName("conversationId")] string? ConversationId,
    [property: JsonPropertyName("content")] ContentDto? Content,
    [property: JsonPropertyName("concluded")] ContentConcludedDto? Concluded,
    [property: JsonPropertyName("summary")] string? Summary,
    [property: JsonPropertyName("error")] string? Error);