using System.Text.Json.Serialization;
using Domain.Dto.Conversation;

namespace Domain.Dto.Chat.Stream;

public class ContentConcludedDto : PromptDto
{
    [JsonPropertyName("messageId")]
    public required string MessageId { get; init; }
}
