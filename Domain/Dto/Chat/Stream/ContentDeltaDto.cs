using System.Text.Json.Serialization;
using Domain.Dto.Conversation;

namespace Domain.Dto.Chat.Stream;

public record ContentDeltaDto(
    [property: JsonPropertyName("conversationId")] string? ConversationId,
    [property: JsonPropertyName("userMessageId")] string? UserMessageId,
    [property: JsonPropertyName("content")] StreamContentDto? Content,
    [property: JsonPropertyName("concluded")] ContentConcludedDto? Concluded,
    [property: JsonPropertyName("summary")] string? Summary,
    [property: JsonPropertyName("error")] string? Error);