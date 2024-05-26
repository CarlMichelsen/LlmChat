namespace Domain.Configuration;

public class LargeLanguageModelOptions
{
    public const string SectionName = "LargeLanguageModel";

    public required string Url { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }
}
