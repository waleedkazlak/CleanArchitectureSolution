using System.Diagnostics;

namespace CleanSample.Presentation.Middleware;

/// <summary>
/// Middleware for logging HTTP requests and responses
/// </summary>
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    // Paths to exclude from detailed logging (static files, health checks, etc.)
    private static readonly string[] ExcludedPaths = new[]
    {
        "/swagger",
        "/health",
        "/metrics",
        "/.well-known",
        "/favicon.ico",
        "/css",
        "/js",
        "/images",
        "/fonts"
    };

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if this path should be excluded from logging
        if (ShouldExcludePath(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var request = context.Request;

        // Log request details
        _logger.LogInformation(
            "HTTP Request: {Method} {Path} | IP: {IpAddress} | User: {User}",
            request.Method,
            request.Path,
            GetClientIpAddress(context),
            context.User?.Identity?.Name ?? "Anonymous");

        // Log request headers (excluding sensitive ones)
        LogRequestHeaders(request);

        var originalBodyStream = context.Response.Body;

        try
        {
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                stopwatch.Stop();

                // Log response details
                _logger.LogInformation(
                    "HTTP Response: {Method} {Path} | Status: {StatusCode} | Duration: {ElapsedMilliseconds}ms",
                    request.Method,
                    request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);

                // Copy response body back to original stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(ex,
                "Exception in HTTP Request: {Method} {Path} | Duration: {ElapsedMilliseconds}ms",
                request.Method,
                request.Path,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }

    private static bool ShouldExcludePath(PathString path)
    {
        var pathValue = path.Value?.ToLower() ?? string.Empty;
        return ExcludedPaths.Any(excluded => pathValue.StartsWith(excluded.ToLower()));
    }

    private static string GetClientIpAddress(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            var ip = forwardedFor.ToString().Split(',').First().Trim();
            return ip;
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    private void LogRequestHeaders(HttpRequest request)
    {
        // List of sensitive headers to exclude
        var sensitiveHeaders = new[] { "Authorization", "Cookie", "Password", "Token", "API-Key" };

        var headersToLog = request.Headers
            .Where(h => !sensitiveHeaders.Any(s => h.Key.Equals(s, StringComparison.OrdinalIgnoreCase)))
            .Select(h => $"{h.Key}: {h.Value}")
            .ToList();

        if (headersToLog.Any())
        {
            _logger.LogDebug("Request Headers: {Headers}", string.Join(" | ", headersToLog));
        }
    }
}