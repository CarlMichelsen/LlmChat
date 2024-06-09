using Domain.Entity.Id;

namespace Domain.Entity;

public class ConversationEntity
{
    public required ConversationEntityId Id { get; init; }

    public string? Summary { get; set; }

    public required Guid CreatorIdentifier { get; init; }

    public required List<MessageEntity> Messages { get; init; }

    public required DateTime LastAppendedUtc { get; set; }

    public required DateTime CreatedUtc { get; init; }
}
