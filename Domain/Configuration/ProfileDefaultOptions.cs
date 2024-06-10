namespace Domain.Configuration;

public class ProfileDefaultOptions
{
    public const string SectionName = "ProfileDefaults";

    public required string SystemMessage { get; init; }

    public required Guid ModelIdentifier { get; init; }
}
