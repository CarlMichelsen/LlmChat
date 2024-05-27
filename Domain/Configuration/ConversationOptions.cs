namespace Domain.Configuration;

public class ConversationOptions
{
    public const string SectionName = "Conversation";

    public required Guid SummaryModelIdentifier { get; init; }
}
