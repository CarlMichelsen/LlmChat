using Domain.Abstraction;

namespace Domain.Entity.Id;

public class ProfileEntityId : TypedGuid<ProfileEntityId>
{
    public ProfileEntityId(Guid value)
        : base(value)
    {
    }
}
