﻿using System.Text.Json;

namespace Domain.Configuration;

public static class ApplicationConstants
{
    public const string AccessCookieName = "Access";

    public const string SessionAuthenticationScheme = "RequireSessionScheme";

    public const string DevelopmentCorsPolicyName = "DevelopmentCors";

    public static JsonSerializerOptions DefaultJsonOptions => new(JsonSerializerOptions.Default) { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}