using System.Text.Json.Serialization;

namespace Domain.Dto.Conversation;

public class ConversationOptionDto
{
    [JsonPropertyName("id")]
    public required long Id { get; init; }

    [JsonPropertyName("summary")]
    public required string? Summary { get; init; }

    [JsonPropertyName("lastAppendedUtc")]
    public required DateTime LastAppendedUtc { get; init; }

    [JsonPropertyName("createdUtc")]
    public required DateTime CreatedUtc { get; init; }
}
