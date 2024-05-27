using System.Text.Json.Serialization;

namespace Domain.Dto.Conversation;

public class ConversationDto
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("summary")]
    public required string? Summary { get; init; }

    [JsonPropertyName("dialogSlices")]
    public required List<DialogSliceDto> DialogSlices { get; init; }

    [JsonPropertyName("lastUpdatedUtc")]
    public required DateTime LastUpdatedUtc { get; init; }

    [JsonPropertyName("createdUtc")]
    public required DateTime CreatedUtc { get; init; }
}
