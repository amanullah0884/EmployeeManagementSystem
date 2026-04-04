namespace EmployeeManagement.Shared.Api;

public class ErrorDetails
{
    public string? Field { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? Code { get; set; }
}
