using App.Security;
using Domain.Configuration;
using Implementation.Service;
using Interface.Service;
using Microsoft.AspNetCore.Authentication;

namespace App;

public static class Dependencies
{
    public static void RegisterApplicationDependencies(this WebApplicationBuilder builder)
    {
        // Configuration
        builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
        builder.Services
            .Configure<RedisOptions>(builder.Configuration.GetSection(RedisOptions.SectionName));

        // Service
        builder.Services
            .AddScoped<ICacheService, CacheService>()
            .AddScoped<ISessionService, SessionService>();
        
        // Client
        builder.Services
            .AddHttpContextAccessor();
        
        // Cache
        var redisConfiguration = builder.Configuration.GetSection(RedisOptions.SectionName);
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfiguration[nameof(RedisOptions.ConnectionString)];
            options.InstanceName = redisConfiguration[nameof(RedisOptions.InstanceName)];
        });

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                ApplicationConstants.DevelopmentCorsPolicyName,
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5539")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });

        // Access Control
        builder.Services.AddControllers();
        builder.Services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, SessionAuthenticationHandler>(ApplicationConstants.SessionAuthenticationScheme, options => { });
        
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(ApplicationConstants.SessionAuthenticationScheme, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimConstants.UserProfileId);
                policy.RequireClaim(ClaimConstants.AuthenticationMethodUserId);
                policy.RequireClaim(ClaimConstants.Name);
                policy.RequireClaim(ClaimConstants.Email);
                policy.RequireClaim(ClaimConstants.AuthenticationMethod);
                policy.RequireClaim(ClaimConstants.AuthenticationMethodName);
            });
        });
    }
}
