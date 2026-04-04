using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _db;

    public AuditLogRepository(AppDbContext db)
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

    public void Add(AuditLog entity) => _db.AuditLogs.Add(entity);
}
