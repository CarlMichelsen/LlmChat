using System.Text.Json.Serialization;

namespace Domain.Dto.Chat.Stream;

public class ContentDto
{
    [JsonPropertyName("index")]
    public required int Index { get; init; }

    [JsonPropertyName("contentType")]
    public required string ContentType { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }
}