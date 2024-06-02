namespace Domain.Entity;

public class ConversationEntity
{
    public long Id { get; set; }

    public string? Summary { get; set; }

    public required Guid CreatorIdentifier { get; init; }

    public required List<MessageEntity> Messages { get; init; }

    public required DateTime LastAppendedUtc { get; set; }

    public required DateTime CreatedUtc { get; init; }
}
