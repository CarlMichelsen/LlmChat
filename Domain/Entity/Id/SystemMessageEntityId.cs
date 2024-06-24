using Domain.Abstraction;

namespace Domain.Entity.Id;

public class SystemMessageEntityId(Guid value) : TypedGuid<SystemMessageEntityId>(value);
