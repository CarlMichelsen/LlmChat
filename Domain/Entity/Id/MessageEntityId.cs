using Domain.Abstraction;

namespace Domain.Entity.Id;

public class MessageEntityId : TypedGuid<MessageEntityId>
{
    public MessageEntityId(Guid value)
        : base(value)
    {
    }
}