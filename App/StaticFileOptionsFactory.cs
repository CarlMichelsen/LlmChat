namespace App;

public static class StaticFileOptionsFactory
{
    public static StaticFileOptions Create()
    {
        return new StaticFileOptions
        {
            ServeUnknownFileTypes = true,
            DefaultContentType = "application/octet-stream",
            OnPrepareResponse = context =>
            {
                var oneWeek = 604800;
                var path = context.Context.Request.Path.Value!;
                if (path.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
                {
                    context.Context.Response.Headers.Append("Cache-Control", $"public, max-age={oneWeek * 53}");
                }
                else if (path.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
                {
                    context.Context.Response.Headers.Append("Cache-Control", $"public, max-age={oneWeek * 53}");
                }
                else
                {
                    context.Context.Response.Headers.Append("Cache-Control", $"public, max-age={oneWeek * 53}");
                }
            },
        };
    }
}
