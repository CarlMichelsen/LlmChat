using Domain.Entity.Id;

namespace Domain.Entity;

public class MessageEntity
{
    public required MessageEntityId Id { get; init; }

    public required List<ContentEntity> Content { get; init; }

    public required PromptEntity? Prompt { get; set; }

    public required MessageEntity? PreviousMessage { get; set; }

    public required DateTime CompletedUtc { get; set; }
}
