using Domain.Entity.Id;

namespace Domain.Pipeline.SendMessage;

public class ResponseToData
{
    public required ConversationEntityId ConversationId { get; init; }

    public required MessageEntityId? MessageId { get; init; }
}
