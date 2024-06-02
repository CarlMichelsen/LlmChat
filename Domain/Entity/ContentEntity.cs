namespace Domain.Entity;

public class ContentEntity
{
    public long Id { get; set; }

    public required MessageContentType ContentType { get; init; }
    
    public required string Content { get; set; }
}
