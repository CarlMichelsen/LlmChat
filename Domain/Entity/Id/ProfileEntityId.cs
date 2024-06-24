using Domain.Abstraction;

namespace Domain.Entity.Id;

public class ProfileEntityId(Guid value) : TypedGuid<ProfileEntityId>(value);