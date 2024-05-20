namespace Domain.Session;

public record SessionData(
    Guid SessionDataKey,
    int AuthenticationMethod,
    string AuthenticationMethodName,
    object CodeResponse,
    UserData User,
    Guid UserProfileId,
    DateTime SessionLastUpdatedUtc);