using Domain.Abstraction;

namespace Domain.Entity.Id;

public class PromptEntityId(Guid value) : TypedGuid<PromptEntityId>(value);