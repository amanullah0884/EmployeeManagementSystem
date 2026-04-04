namespace EmployeeManagement.Shared.Api;

/// <summary>
/// Standard API envelope for all HTTP responses.
/// </summary>
public class StandardResponse
{
    public bool IsSuccess { get; set; }

    public string StatusCode { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public object? Data { get; set; }

    public List<ErrorDetails>? Errors { get; set; }
}
