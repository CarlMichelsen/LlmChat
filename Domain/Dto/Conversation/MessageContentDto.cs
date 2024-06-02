using System.Text.Json.Serialization;

namespace Domain.Dto.Conversation;

public class MessageContentDto
{
    [JsonPropertyName("contentType")]
    public required string ContentType { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }
}
