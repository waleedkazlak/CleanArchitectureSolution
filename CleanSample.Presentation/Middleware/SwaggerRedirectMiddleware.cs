namespace CleanSample.Presentation.Middleware;

/// <summary>
/// Middleware to redirect root path to Swagger UI (used in production or if needed)
/// </summary>
public class SwaggerRedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SwaggerRedirectMiddleware> _logger;

    public SwaggerRedirectMiddleware(RequestDelegate next, ILogger<SwaggerRedirectMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only redirect root path requests
        if (context.Request.Path == "/" && !context.Request.QueryString.HasValue)
        {
            _logger.LogInformation("Redirecting root path to Swagger UI");
            context.Response.Redirect("/swagger/index.html", permanent: false);
            return;
        }

        await _next(context);
    }
}   