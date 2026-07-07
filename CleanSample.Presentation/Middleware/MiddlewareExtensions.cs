namespace CleanSample.Presentation.Middleware;

/// <summary>
/// Extension methods for middleware
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Use request logging middleware (basic HTTP logging)
    /// </summary>
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoggingMiddleware>();
    }

    /// <summary>
    /// Use structured logging middleware (advanced with correlation IDs and performance metrics)
    /// </summary>
    public static IApplicationBuilder UseStructuredLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<StructuredLoggingMiddleware>();
    }

    /// <summary>
    /// Use exception handling middleware
    /// </summary>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    /// <summary>
    /// Use Swagger redirect middleware
    /// </summary>
    public static IApplicationBuilder UseSwaggerRedirect(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SwaggerRedirectMiddleware>();
    }
}