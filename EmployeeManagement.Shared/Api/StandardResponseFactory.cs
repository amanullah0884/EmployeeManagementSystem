namespace EmployeeManagement.Shared.Api;

public static class StandardResponseFactory
{
    public static StandardResponse Success(object? data, string message, string statusCode = "200") =>
        new()
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Message = message,
            Data = data,
            Errors = null
        };

    public static StandardResponse Failure(string message, string statusCode, IEnumerable<ErrorDetails>? errors = null) =>
        new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message,
            Data = null,
            Errors = errors?.ToList() ?? new List<ErrorDetails>()
        };

    public static StandardResponse Failure(string message, string statusCode, string errorMessage, string? field = null) =>
        Failure(message, statusCode, new[]
        {
            new ErrorDetails { Field = field, Message = errorMessage }
        });
}
