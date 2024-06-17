using System.Text.Json.Serialization;

namespace Domain.Dto.Conversation;

public class MessageDto
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("isUserMessage")]
    public required bool IsUserMessage { get; init; }
    
    [JsonPropertyName("prompt")]
    public required PromptDto? Prompt { get; init; }

    [JsonPropertyName("content")]
    public required List<MessageContentDto> Content { get; init; }

    [JsonPropertyName("completedUtc")]
    public required DateTime CompletedUtc { get; init; }

    [JsonPropertyName("previousMessageId")]
    public required Guid? PreviousMessageId { get; init; }
}
