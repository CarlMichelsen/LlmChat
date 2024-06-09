using Domain.Abstraction;

namespace Domain.Entity.Id;

public class PromptEntityId : TypedGuid<PromptEntityId>
{
    public PromptEntityId(Guid value)
        : base(value)
    {
    }
}