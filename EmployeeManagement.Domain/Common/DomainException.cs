namespace EmployeeManagement.Domain.Common;

/// <summary>
/// Thrown when a domain rule is violated. No infrastructure or persistence dependencies.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
