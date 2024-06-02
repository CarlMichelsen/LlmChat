using Domain.Entity;

namespace Domain.Conversation;

public class NewMessageData
{
    public required ConversationEntity Conversation { get; init; }

    public required MessageEntity Message { get; init; }
}