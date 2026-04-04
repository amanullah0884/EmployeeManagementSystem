using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _db;

    public DepartmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Department>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Departments
            .AsNoTracking()
            .Include(d => d.Employees)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken cancellationToken = default)
    {
        var q = _db.Departments.Where(d => d.Name == name);
        if (excludeId is { } id)
            q = q.Where(d => d.Id != id);

        return await q.AnyAsync(cancellationToken);
    }

    public void Add(Department entity) => _db.Departments.Add(entity);

    public void Update(Department entity) => _db.Departments.Update(entity);

    public void Remove(Department entity) => _db.Departments.Remove(entity);
}
