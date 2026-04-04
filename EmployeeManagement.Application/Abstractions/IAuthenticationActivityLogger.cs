namespace EmployeeManagement.Application.Abstractions;

public interface IAuthenticationActivityLogger
{
    Task LogLoginSuccessAsync(int userId, string username, string? correlationId, CancellationToken cancellationToken = default);

    Task LogLoginFailureAsync(string username, string reason, string? correlationId, CancellationToken cancellationToken = default);

    Task LogLogoutAsync(int userId, string? username, string? correlationId, CancellationToken cancellationToken = default);
}
