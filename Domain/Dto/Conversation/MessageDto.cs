using System.Text.Json.Serialization;

namespace Domain.Dto.Conversation;

public class MessageDto
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("content")]
    public required List<MessageContentDto> Content { get; init; }

    [JsonPropertyName("completedUtc")]
    public required DateTime CompletedUtc { get; init; }
}
