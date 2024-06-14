using Domain.Abstraction;

namespace Domain.Entity.Id;

public class DialogSliceEntityId : TypedGuid<DialogSliceEntityId>
{
    public DialogSliceEntityId(Guid value)
        : base(value)
    {
    }
}
