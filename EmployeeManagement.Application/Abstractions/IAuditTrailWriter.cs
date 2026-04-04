namespace EmployeeManagement.Application.Abstractions;

/// <summary>
/// Persists structured audit records (database-driven activity trail).
/// </summary>
public interface IAuditTrailWriter
{
    Task WriteAsync(
        string eventCategory,
        string action,
        string entityName,
        string? entityId,
        string? details,
        int? userId,
        string? username,
        string? correlationId,
        CancellationToken cancellationToken = default);
}
