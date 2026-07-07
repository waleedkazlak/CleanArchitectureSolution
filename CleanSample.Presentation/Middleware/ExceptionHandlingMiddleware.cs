using CleanSample.Application.DTOs;
using CleanSample.Application.Exceptions;
using System.Net;

namespace CleanSample.Presentation.Middleware;

/// <summary>
/// Middleware for handling exceptions and returning proper error responses
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation exception occurred: {@Errors}", ex.Errors);
            await HandleValidationException(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected exception occurred");
            await HandleException(context, ex);
        }
    }

    private static Task HandleValidationException(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        var response = new APIBaseResponse<object>
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity,
            IsSuccess = false,
            ErrorList = exception.Errors.Values.SelectMany(v => v).ToList(),
            ValidationErrorList = exception.Errors
        };

        return context.Response.WriteAsJsonAsync(response);
    }

    private static Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        object? validationErrors = null;
        if (exception is FluentValidation.ValidationException validationException && ((FluentValidation.ValidationException)validationException).Errors.Count() > 0)
        {
            validationErrors = validationException.Errors;
        }

        var response = new APIBaseResponse<object>
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            IsSuccess = false,
            ValidationErrorList = validationErrors,
            ErrorList = validationErrors != null ? null : new List<string> { "An unexpected error occurred. Please try again later." }
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}