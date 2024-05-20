using System.Security.Claims;
using System.Text.Encodings.Web;
using Domain.Configuration;
using Domain.Session;
using Interface.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace App.Security;

public class SessionAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ISessionService sessionService;

    public SessionAuthenticationHandler(
        ISessionService sessionService,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
        this.sessionService = sessionService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = this.Context.GetEndpoint();
        var authorizeData = endpoint?.Metadata?.GetMetadata<IAuthorizeData>();

        if (authorizeData is null || authorizeData.AuthenticationSchemes?.Any() == false)
        {
            // If the endpoint doesn't require authorization, skip authentication
            return AuthenticateResult.NoResult();
        }

        var sessionDataResult = await this.sessionService.GetSessionData();
        if (sessionDataResult.IsSuccess)
        {
            var claimsPrincipal = this.CreateClaimsPrincipal(sessionDataResult.Unwrap());
            var ticket = new AuthenticationTicket(claimsPrincipal, ApplicationConstants.SessionAuthenticationScheme);
            return AuthenticateResult.Success(ticket);
        }
        
        return AuthenticateResult.NoResult();
    }

    private ClaimsPrincipal CreateClaimsPrincipal(
        SessionData sessionData)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimConstants.UserProfileId, sessionData.UserProfileId.ToString()),
            new Claim(ClaimConstants.AuthenticationMethodUserId, sessionData.User.Id),
            new Claim(ClaimConstants.Name, sessionData.User.Name),
            new Claim(ClaimConstants.Email, sessionData.User.Email),
            new Claim(ClaimConstants.AuthenticationMethod, sessionData.AuthenticationMethod.ToString()),
            new Claim(ClaimConstants.AuthenticationMethodName, sessionData.AuthenticationMethodName),
        };

        var identity = new ClaimsIdentity(claims, "session");
        return new ClaimsPrincipal(identity);
    }
}