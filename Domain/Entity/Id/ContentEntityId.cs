using Domain.Abstraction;

namespace Domain.Entity.Id;

public class ContentEntityId(Guid value) : TypedGuid<ContentEntityId>(value);
