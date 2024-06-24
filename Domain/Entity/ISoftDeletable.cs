namespace Domain.Entity;

public interface ISoftDeletable
{
    DateTime? DeletedAtUtc { get; }
}
