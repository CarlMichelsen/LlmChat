using Domain.Abstraction;

namespace Domain.Entity.Id;

public class ConversationEntityId(Guid value) : TypedGuid<ConversationEntityId>(value);
