using System.Text.Json.Serialization;

namespace Domain.Dto.Chat;

public class ResponseToDto
{
    [JsonPropertyName("conversationId")]
    public required Guid ConversationId { get; init; }

    [JsonPropertyName("responseToMessageId")]
    public required Guid ResponseToMessageId { get; init; }
}
