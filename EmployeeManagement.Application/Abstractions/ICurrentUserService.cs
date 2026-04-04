namespace EmployeeManagement.Application.Abstractions;

public interface ICurrentUserService
{
    int? UserId { get; }

    string? Username { get; }

    IReadOnlyList<string> Permissions { get; }
}
