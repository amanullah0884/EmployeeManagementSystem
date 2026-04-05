using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly LoggingDbContext _db;

    public AuditLogRepository(LoggingDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<AuditLog>> ListRecentAsync(int take, CancellationToken cancellationToken = default)
    {
        return await _db.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAtUtc)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}
