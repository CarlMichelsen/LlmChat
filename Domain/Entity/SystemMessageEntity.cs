using Domain.Entity.Id;

namespace Domain.Entity;

public class SystemMessageEntity : ISoftDeletable
{
    public required SystemMessageEntityId Id { get; init; }

    public required ProfileEntity Creator { get; init; }

    public required string Name { get; set; }

    public required string Content { get; set; }

    public required DateTime LastAppendedUtc { get; set; }

    public required DateTime CreatedUtc { get; init; }
    
    public DateTime? DeletedAtUtc { get; set; }
}
