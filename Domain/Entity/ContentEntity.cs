using Domain.Entity.Id;

namespace Domain.Entity;

public class ContentEntity
{
    public required ContentEntityId Id { get; init; }

    public required MessageContentType ContentType { get; init; }
    
    public required string Content { get; set; }
}
