using Domain.Dto.Conversation;

namespace Domain.Conversation;

public class ConcludedMessage
{
    public required long ConversationId { get; init; }
    
    public required string ProviderPromptIdentifier { get; init; }

    public required string ModelIdentifierName { get; init; }

    public required long InputTokens { get; init; }

    public required long OutputTokens { get; init; }

    public required long CurrentMillionInputTokenPrice { get; init; }

    public required long CurrentMillionOutputTokenPrice { get; init; }

    public required List<MessageContentDto> Content { get; init; }

    public required NewMessageData NewMessageData { get; init; }

    public required StreamUsage? StreamUsage { get; init; }
}