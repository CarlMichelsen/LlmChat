using Domain.Abstraction;

namespace Domain.Entity.Id;

public class DialogSliceEntityId(Guid value) : TypedGuid<DialogSliceEntityId>(value);