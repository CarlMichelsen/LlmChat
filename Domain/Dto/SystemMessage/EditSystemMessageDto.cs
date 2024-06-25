using System.Text.Json.Serialization;

namespace Domain.Dto.SystemMessage;

public class EditSystemMessageDto
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("content")]
    public string? Content { get; init; }
}
