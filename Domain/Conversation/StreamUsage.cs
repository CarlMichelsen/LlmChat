namespace Domain.Conversation;

public class StreamUsage
{
    public required string ProviderPromptIdentifier { get; init; }

    public required long InputTokens { get; init; }

    public required long OutputTokens { get; init; }

    public required string StopReason { get; init; }
}
