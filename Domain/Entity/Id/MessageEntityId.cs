using Domain.Abstraction;

namespace Domain.Entity.Id;

public class MessageEntityId(Guid value) : TypedGuid<MessageEntityId>(value);