using System.Text.Json.Serialization;

namespace Domain.Dto.Conversation;

public class ConversationOptionDto
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("summary")]
    public required string? Summary { get; init; }

    [JsonPropertyName("lastAppendedEpoch")]
    public required long LastAppendedEpoch { get; init; }

    [JsonPropertyName("createdEpoch")]
    public required long CreatedEpoch { get; init; }
}
