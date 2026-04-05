using EmployeeManagement.Application.Abstractions;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;

namespace EmployeeManagement.Persistence.Audit;

public class AuditTrailWriter : IAuditTrailWriter
{
    private readonly LoggingDbContext _db;

    public AuditTrailWriter(LoggingDbContext db)
    {
        _db = db;
    }

    public async Task WriteAsync(
        string eventCategory,
        string action,
        string entityName,
        string? entityId,
        string? details,
        int? userId,
        string? username,
        string? correlationId,
        CancellationToken cancellationToken = default)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            EventCategory = eventCategory,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Details = details,
            UserId = userId,
            Username = username,
            CorrelationId = correlationId,
            CreatedAtUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync(cancellationToken);
    }
}
