using System.Diagnostics;
using System.Text;

namespace CleanSample.Presentation.Middleware;

/// <summary>
/// Middleware for structured logging of HTTP requests and responses with performance metrics
/// </summary>
public class StructuredLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<StructuredLoggingMiddleware> _logger;

    // Paths to exclude from detailed logging
    private static readonly string[] ExcludedPaths = new[]
    {
        "/swagger",
        "/health",
        "/metrics",
        "/.well-known",
        "/favicon.ico"
    };

    public StructuredLoggingMiddleware(RequestDelegate next, ILogger<StructuredLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip logging for excluded paths
        if (ShouldExcludePath(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var correlationId = GetOrCreateCorrelationId(context);
        context.Items["CorrelationId"] = correlationId;

        var stopwatch = Stopwatch.StartNew();
        var request = context.Request;

        // Capture request body if it's readable
        string? requestBody = null;
        if (request.Method != "GET" && request.ContentLength > 0)
        {
            try
            {
                request.EnableBuffering();
                var body = await new StreamReader(request.Body).ReadToEndAsync();
                request.Body.Position = 0;
                requestBody = body.Length > 1000 ? body.Substring(0, 1000) + "..." : body;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read request body");
            }
        }

        using var responseBody = new MemoryStream();
        var originalBodyStream = context.Response.Body;
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
            stopwatch.Stop();

            // Capture response body
            var body = await ReadResponseBody(responseBody);

            // Log structured information
            _logger.LogInformation(
                "HTTP Request Completed | CorrelationId: {CorrelationId} | Method: {Method} | Path: {Path} | StatusCode: {StatusCode} | Duration: {DurationMs}ms | User: {User} | IP: {IpAddress}",
                correlationId,
                request.Method,
                request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                context.User?.Identity?.Name ?? "Anonymous",
                GetClientIpAddress(context));

            // Log response body for errors only
            if (context.Response.StatusCode >= 400 && !string.IsNullOrEmpty(body))
            {
                _logger.LogWarning("Error Response Body: {Body}", 
                    body.Length > 500 ? body.Substring(0, 500) + "..." : body);
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(ex,
                "HTTP Request Failed | CorrelationId: {CorrelationId} | Method: {Method} | Path: {Path} | Duration: {DurationMs}ms | User: {User}",
                correlationId,
                request.Method,
                request.Path,
                stopwatch.ElapsedMilliseconds,
                context.User?.Identity?.Name ?? "Anonymous");

            throw;
        }
        finally
        {
            // Ensure response body is written to original stream
            if (context.Response.Body != originalBodyStream)
            {
                context.Response.Body = originalBodyStream;
            }
        }
    }

    private static bool ShouldExcludePath(PathString path)
    {
        var pathValue = path.Value?.ToLower() ?? string.Empty;
        return ExcludedPaths.Any(excluded => pathValue.StartsWith(excluded.ToLower()));
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        const string correlationIdHeader = "X-Correlation-ID";

        if (context.Request.Headers.TryGetValue(correlationIdHeader, out var correlationId))
        {
            return correlationId.ToString();
        }

        var newCorrelationId = Guid.NewGuid().ToString();
        context.Response.Headers.Add(correlationIdHeader, newCorrelationId);
        return newCorrelationId;
    }

    private static string GetClientIpAddress(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            return forwardedFor.ToString().Split(',').First().Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    private static async Task<string> ReadResponseBody(MemoryStream responseBody)
    {
        try
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(responseBody, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);
            return body;
        }
        catch
        {
            return string.Empty;
        }
    }
}