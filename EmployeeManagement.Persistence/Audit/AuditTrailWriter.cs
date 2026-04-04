using EmployeeManagement.Application.Abstractions;
using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Persistence.Audit;

public class AuditTrailWriter : IAuditTrailWriter
{
    private readonly IUnitOfWork _unitOfWork;

    public AuditTrailWriter(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        _unitOfWork.AuditLogs.Add(new AuditLog
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

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
