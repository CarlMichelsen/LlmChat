using Domain.Entity.Id;

namespace Domain.Entity;

public class PromptEntity
{
    public required PromptEntityId Id { get; init; }

    public required string ModelName { get; init; }

    public required Guid ModelId { get; set; }

    public required string ProviderPromptIdentifier { get; set; }

    public required string Model { get; init; }

    public required long InputTokens { get; init; }

    public required long OutputTokens { get; init; }

    public required long CurrentMillionInputTokenPrice { get; init; }

    public required long CurrentMillionOutputTokenPrice { get; init; }

    public required string StopReason { get; init; }
}
