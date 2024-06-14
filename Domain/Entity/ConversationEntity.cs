using Domain.Entity.Id;

namespace Domain.Entity;

public class ConversationEntity
{
    public required ConversationEntityId Id { get; init; }

    public string? Summary { get; set; }

    public required string SystemMessage { get; set; }

    public required ProfileEntity Creator { get; init; }

    public required List<DialogSliceEntity> DialogSlices { get; set; }

    public required DateTime LastAppendedUtc { get; set; }

    public required DateTime CreatedUtc { get; init; }
}
