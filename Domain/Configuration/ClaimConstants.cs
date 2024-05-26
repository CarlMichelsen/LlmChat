using System.Security.Claims;

namespace Domain.Configuration;

public static class ClaimConstants
{
    public const string SessionIdentityName = "SessionIdentity";

    public const string UserProfileId = "ProfileId";

    public const string AuthenticationMethodUserId = "AuthenticationMethodUserId";

    public const string Name = ClaimTypes.Name;

    public const string Email = ClaimTypes.Email;

    public const string AuthenticationMethod = "AuthenticationMethod";

    public const string AuthenticationMethodName = "AuthenticationMethodName";
}
