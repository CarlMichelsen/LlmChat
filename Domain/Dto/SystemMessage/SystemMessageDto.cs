using System.Text.Json.Serialization;

namespace Domain.Dto.SystemMessage;

public class SystemMessageDto
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }

    [JsonPropertyName("lastAppendedUtc")]
    public required long LastAppendedUtc { get; init; }

    [JsonPropertyName("createdUtc")]
    public required long CreatedUtc { get; init; }
}
