using EmployeeManagement.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Persistence.Logging;

public class AuthenticationActivityLogger : IAuthenticationActivityLogger
{
    private readonly IAuditTrailWriter _auditTrailWriter;
    private readonly ILogger<AuthenticationActivityLogger> _logger;

    public AuthenticationActivityLogger(IAuditTrailWriter auditTrailWriter, ILogger<AuthenticationActivityLogger> logger)
    {
        _auditTrailWriter = auditTrailWriter;
        _logger = logger;
    }

    public async Task LogLoginSuccessAsync(int userId, string username, string? correlationId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Login success for user {Username} ({UserId}). CorrelationId={CorrelationId}", username, userId, correlationId);
        await _auditTrailWriter.WriteAsync(
            "Authentication",
            "Login",
            "User",
            userId.ToString(),
            "Login succeeded",
            userId,
            username,
            correlationId,
            cancellationToken);
    }

    public async Task LogLoginFailureAsync(string username, string reason, string? correlationId, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Login failed for user {Username}: {Reason}. CorrelationId={CorrelationId}", username, reason, correlationId);
        await _auditTrailWriter.WriteAsync(
            "Authentication",
            "LoginFailed",
            "User",
            username,
            reason,
            null,
            username,
            correlationId,
            cancellationToken);
    }

    public async Task LogLogoutAsync(int userId, string? username, string? correlationId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Logout for user {Username} ({UserId}). CorrelationId={CorrelationId}", username, userId, correlationId);
        await _auditTrailWriter.WriteAsync(
            "Authentication",
            "Logout",
            "User",
            userId.ToString(),
            "Client should discard JWT",
            userId,
            username,
            correlationId,
            cancellationToken);
    }
}
