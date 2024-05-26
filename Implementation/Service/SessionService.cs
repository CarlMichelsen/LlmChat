using System.Security.Claims;
using System.Text.Json;
using Domain.Abstraction;
using Domain.Configuration;
using Domain.Exception;
using Domain.Session;
using Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Implementation.Service;

public class SessionService : ISessionService
{
    private readonly ILogger<SessionService> logger;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ICacheService cacheService;

    public SessionService(
        ILogger<SessionService> logger,
        IHttpContextAccessor httpContextAccessor,
        ICacheService cacheService)
    {
        this.logger = logger;
        this.httpContextAccessor = httpContextAccessor;
        this.cacheService = cacheService;
    }

    public Guid UserProfileId
    {
        get
        {
            var claimsIdentity = this.httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
            var userProfileId = claimsIdentity?.FindFirst(ClaimConstants.UserProfileId)?.Value;
            return Guid.Parse(userProfileId!);
        }
    }

    public async Task<Result<SessionData>> GetSessionData()
    {
        try
        {
            var httpContext = this.httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                return new SafeUserFeedbackException("GetSessionData.NoHttpContext");
            }

            var cookieDataString = httpContext.Request.Cookies[ApplicationConstants.AccessCookieName];
            if (cookieDataString is null)
            {
                return new SafeUserFeedbackException("GetSessionData.NoAccessCookie");
            }

            var cookieData = JsonSerializer.Deserialize<CookieData>(
                cookieDataString,
                ApplicationConstants.DefaultJsonOptions);
            if (cookieData is null)
            {
                return new SafeUserFeedbackException("GetSessionData.UnserializableAccessCookie");
            }

            var sessionDataString = await this.cacheService
                .Get(cookieData.SessionCacheKey.ToString());
            
            if (string.IsNullOrWhiteSpace(sessionDataString))
            {
                return new SafeUserFeedbackException("GetSessionData.NoSessionData");
            }
            
            var sessionData = JsonSerializer.Deserialize<SessionData>(
                sessionDataString,
                ApplicationConstants.DefaultJsonOptions);
            if (sessionData is null)
            {
                return new SafeUserFeedbackException("GetSessionData.UnserializableSessionData");
            }

            return sessionData;
        }
        catch (Exception e)
        {
            this.logger.LogCritical(e, "An exception occured when attempting to fetch sessionData");
            return new SafeUserFeedbackException($"GetSessionData.Exception\n{e.Message}");
        }
    }

    public async Task<Result<bool>> RemoveSessionData()
    {
        try
        {
            var httpContext = this.httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                return new SafeUserFeedbackException("RemoveSessionData.NoHttpContext");
            }

            var cookieDataString = httpContext.Request.Cookies[ApplicationConstants.AccessCookieName];
            if (cookieDataString is null)
            {
                return new SafeUserFeedbackException("RemoveSessionData.NoAccessCookie");
            }

            var cookieData = JsonSerializer.Deserialize<CookieData>(
                cookieDataString,
                ApplicationConstants.DefaultJsonOptions);
            if (cookieData is null)
            {
                return new SafeUserFeedbackException("RemoveSessionData.UnserializableAccessCookie");
            }

            await this.cacheService.Remove(cookieData.SessionCacheKey.ToString());
            httpContext.Response.Cookies.Delete(ApplicationConstants.AccessCookieName);

            return true;
        }
        catch (Exception e)
        {
            this.logger.LogCritical(e, "An exception occured when attempting to remove sessionData");
            return new SafeUserFeedbackException($"RemoveSessionData.Exception\n{e.Message}");
        }
    }
}