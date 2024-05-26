namespace Domain.Entity;

public class PromptEntity
{
    public long Id { get; set; }

    public required string ProviderPromptIdentifier { get; set; }

    public required string Model { get; init; }

    public required long InputTokens { get; init; }

    public required long OutputTokens { get; init; }

    public required long CurrentMillionInputTokenPrice { get; init; }

    public required long CurrentMillionOutputTokenPrice { get; init; }
}
