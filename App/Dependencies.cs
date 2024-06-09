using App.Extensions;
using App.Security;
using Domain.Configuration;
using Domain.Pipeline.SendMessage;
using Implementation.Database;
using Implementation.Handler;
using Implementation.Pipeline;
using Implementation.Repository;
using Implementation.Service;
using Interface.Handler;
using Interface.Pipeline;
using Interface.Repository;
using Interface.Service;
using LargeLanguageModelClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

namespace App;

public static class Dependencies
{
    public static void RegisterApplicationDependencies(this WebApplicationBuilder builder)
    {
        // Configuration
        builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
        builder.Services
            .Configure<LargeLanguageModelOptions>(builder.Configuration.GetSection(LargeLanguageModelOptions.SectionName))
            .Configure<ConversationOptions>(builder.Configuration.GetSection(ConversationOptions.SectionName))
            .Configure<RedisOptions>(builder.Configuration.GetSection(RedisOptions.SectionName));

        // Handler
        builder.Services
            .AddScoped<ISessionHandler, SessionHandler>()
            .AddScoped<IModelHandler, ModelHandler>()
            .AddScoped<IMessageHandler, MessageHandler>()
            .AddScoped<IConversationHandler, ConversationHandler>();

        // Pipeline
        builder.Services
            .RegisterPipelineSteps()
            .AddTransient<ITransactionPipeline<ApplicationContext, SendMessagePipelineData>, SendMessagePipeline>();

        // Service
        builder.Services
            .AddScoped<ICacheService, CacheService>()
            .AddScoped<ISessionService, SessionService>()
            .AddScoped<IModelService, ModelService>()
            .AddScoped<IStreamWriterService, StreamWriterService>()
            .AddScoped<ISummaryService, SummaryService>()
            .AddScoped<IConversationDtoService, ConversationDtoService>()
            .AddScoped<IConversationOptionService, ConversationOptionService>();
        
        // Repository
        builder.Services
            .AddScoped<IGetOrCreateConversationRepository, GetOrCreateConversationRepository>()
            .AddScoped<IMessageInitiationRepository, MessageInitiationRepository>()
            .AddScoped<IConversationReadRepository, ConversationReadRepository>()
            .AddScoped<IGetOrCreateConversationRepository, GetOrCreateConversationRepository>()
            .AddScoped<IConversationReadRepository, ConversationReadRepository>();
        
        // Client
        var llmConfig = builder.Configuration
            .GetSection(LargeLanguageModelOptions.SectionName)
            .Get<LargeLanguageModelOptions>()!;
        builder.Services
            .RegisterLargeLanguageModelClientDependencies(new Uri(llmConfig.Url), (Username: llmConfig.Username, Password: llmConfig.Password))
            .AddHttpContextAccessor();
        
        // Database
        builder.Services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                (b) => b.MigrationsAssembly("App"));
            
            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
        
        // Cache
        var redisConfiguration = builder.Configuration
            .GetSection(RedisOptions.SectionName)
            .Get<RedisOptions>()!;
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfiguration.ConnectionString;
            options.InstanceName = redisConfiguration.InstanceName;
        });

        // Compression
        builder.Services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                ["application/javascript", "text/css", "text/html", "text/json", "text/plain", "application/json"]);
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
