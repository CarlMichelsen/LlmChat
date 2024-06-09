using Domain.Abstraction;

namespace Domain.Entity.Id;

public class ConversationEntityId : TypedGuid<ConversationEntityId>
{
    public ConversationEntityId(Guid value)
        : base(value)
    {
    }
}
