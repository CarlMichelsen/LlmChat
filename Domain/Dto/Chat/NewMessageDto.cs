using System.Text.Json.Serialization;
using Domain.Dto.Conversation;

namespace Domain.Dto.Chat;

public class NewMessageDto
{
    [JsonPropertyName("responseTo")]
    public ResponseToDto? ResponseTo { get; init; }

    [JsonPropertyName("content")]
    public required List<MessageContentDto> Content { get; init; }

    [JsonPropertyName("modelIdentifier")]
    public required Guid ModelIdentifier { get; init; }
}
