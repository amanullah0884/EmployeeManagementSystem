namespace EmployeeManagement.Shared.Exceptions;

/// <summary>
/// Exception type that can be translated into a standard API envelope by the API layer.
/// </summary>
public class ApiException : Exception
{
    public string StatusCode { get; }

    public ApiException(string message, string statusCode = "400") : base(message)
    {
        StatusCode = statusCode;
    }
}
