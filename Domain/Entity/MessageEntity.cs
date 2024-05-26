namespace Domain.Entity;

public class MessageEntity
{
    public long Id { get; set; }

    public required List<ContentEntity> Content { get; init; }

    public required PromptEntity? Prompt { get; set; }

    public required MessageEntity? PreviousMessage { get; set; }

    public required DateTime CompletedUtc { get; set; }
}
