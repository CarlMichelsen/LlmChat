using Domain.Entity.Id;

namespace Domain.Entity;

public class DialogSliceEntity
{
    public required DialogSliceEntityId Id { get; init; }

    public required List<MessageEntity> Messages { get; init; }

    public required Guid SelectedMessageGuid { get; set; } = Guid.Empty;

    public required DateTime CreatedUtc { get; init; }
}
