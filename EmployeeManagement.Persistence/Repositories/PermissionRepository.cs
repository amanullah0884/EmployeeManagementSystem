using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _db;

    public PermissionRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Permission>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Permissions
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
}
