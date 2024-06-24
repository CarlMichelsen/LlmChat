using Domain.Entity.Id;

namespace Domain.Entity;

public class ProfileEntity
{
    public required ProfileEntityId Id { get; init; }

    public required string DefaultSystemMessage { get; set; }

    public required List<SystemMessageEntity> SystemMessages { get; init; }

    public required Guid SelectedModel { get; set; }
}
