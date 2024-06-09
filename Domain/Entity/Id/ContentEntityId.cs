using Domain.Abstraction;

namespace Domain.Entity.Id;

public class ContentEntityId : TypedGuid<ContentEntityId>
{
    public ContentEntityId(Guid value)
        : base(value)
    {
    }
}
