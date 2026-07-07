using Microsoft.AspNetCore.Http;

namespace CleanSample.Application.DTOs;

/// <summary>
/// Generic base response class for API responses
/// </summary>
/// <typeparam name="T">Type of data in the response</typeparam>
public class APIBaseResponse<T>
{
    /// <summary>
    /// Initializes a new instance of APIBaseResponse
    /// </summary>
    public APIBaseResponse()
    {
        StatusCode = StatusCodes.Status200OK;
        ErrorList = new List<string>();
        IsSuccess = true;
    }

    /// <summary>
    /// The actual response data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Success message
    /// </summary>
    public string? SuccessMessage { get; set; }

    /// <summary>
    /// HTTP status code
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// List of error messages
    /// </summary>
    public List<string> ErrorList { get; set; }

    /// <summary>
    /// Indicates if the operation was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Total records count (useful for pagination)
    /// </summary>
    public int? TotalRecords { get; set; }

    /// <summary>
    /// Validation error details
    /// </summary>
    public object? ValidationErrorList { get; set; }

    /// <summary>
    /// Creates an error response with single message
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="message">Error message</param>
    /// <returns>Current instance for chaining</returns>
    public APIBaseResponse<T> SetError(int statusCode, string message)
    {
        this.StatusCode = statusCode;
        this.IsSuccess = false;
        this.ErrorList.Clear();
        this.ErrorList.Add(message);
        this.Data = default;
        return this;
    }

    /// <summary>
    /// Creates an error response with multiple messages
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="messages">List of error messages</param>
    /// <returns>Current instance for chaining</returns>
    public APIBaseResponse<T> SetError(int statusCode, List<string> messages)
    {
        this.StatusCode = statusCode;
        this.IsSuccess = false;
        this.ErrorList.Clear();
        this.ErrorList.AddRange(messages ?? new List<string>());
        this.Data = default;
        return this;
    }

    /// <summary>
    /// Creates an error response with validation errors
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="message">Error message</param>
    /// <param name="validationErrors">Validation error details</param>
    /// <returns>Current instance for chaining</returns>
    public APIBaseResponse<T> SetError(int statusCode, string message, object validationErrors)
    {
        this.StatusCode = statusCode;
        this.IsSuccess = false;
        this.ErrorList.Clear();
        this.ErrorList.Add(message);
        this.ValidationErrorList = validationErrors;
        this.Data = default;
        return this;
    }

    /// <summary>
    /// Creates a success response
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="message">Success message</param>
    /// <returns>Current instance for chaining</returns>
    public APIBaseResponse<T> SetSuccess(T? data, string message = "Request completed successfully")
    {
        this.Data = data;
        this.SuccessMessage = message;
        this.StatusCode = StatusCodes.Status200OK;
        this.IsSuccess = true;
        this.ErrorList.Clear();
        return this;
    }

    /// <summary>
    /// Creates a success response with custom status code
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="message">Success message</param>
    /// <returns>Current instance for chaining</returns>
    public APIBaseResponse<T> SetSuccess(T? data, int statusCode, string message = "Request completed successfully")
    {
        this.Data = data;
        this.SuccessMessage = message;
        this.StatusCode = statusCode;
        this.IsSuccess = true;
        this.ErrorList.Clear();
        return this;
    }

   
    /// <summary>
    /// Creates a success response with pagination info and custom status code
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="totalRecords">Total records count</param>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="message">Success message</param>
    /// <returns>Current instance for chaining</returns>
    public APIBaseResponse<T> SetSuccess(T? data, int totalRecords, int statusCode, string message = "Request completed successfully")
    {
        this.Data = data;
        this.TotalRecords = totalRecords;
        this.SuccessMessage = message;
        this.StatusCode = statusCode;
        this.IsSuccess = true;
        this.ErrorList.Clear();
        return this;
    }

    /// <summary>
    /// Creates a success response with validation errors
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="validationErrors">Validation errors</param>
    /// <param name="message">Message</param>
    /// <returns>Current instance for chaining</returns>
    public APIBaseResponse<T> SetSuccessWithValidation(T? data, object validationErrors, string message = "Request has validation errors")
    {
        this.Data = data;
        this.ValidationErrorList = validationErrors;
        this.SuccessMessage = message;
        this.StatusCode = StatusCodes.Status422UnprocessableEntity;
        this.IsSuccess = false;
        return this;
    }

    /// <summary>
    /// Static method to create a success response
    /// </summary>
    public static APIBaseResponse<T> Success(
        T? data,
        string message = "Request completed successfully",
        int statusCode = StatusCodes.Status200OK,
        int? totalRecords = null)
    {
        return new APIBaseResponse<T>
        {
            Data = data,
            SuccessMessage = message,
            StatusCode = statusCode,
            TotalRecords = totalRecords,
            IsSuccess = true,
            ErrorList = new List<string>()
        };
    }

    /// <summary>
    /// Static method to create a failure response
    /// </summary>
    public static APIBaseResponse<T> Failure(
        List<string> errors,
        int statusCode,
        object? validationErrors = null)
    {
        return new APIBaseResponse<T>
        {
            IsSuccess = false,
            StatusCode = statusCode,
            ErrorList = errors ?? new List<string>(),
            ValidationErrorList = validationErrors
        };
    }
}