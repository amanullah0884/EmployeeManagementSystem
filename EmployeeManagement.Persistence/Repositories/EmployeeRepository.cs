using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Employees
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Employee>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId, CancellationToken cancellationToken = default)
    {
        var q = _db.Employees.AsQueryable().Where(e => e.Email == email);
        if (excludeId is { } id)
            q = q.Where(e => e.Id != id);

        return await q.AnyAsync(cancellationToken);
    }

    public void Add(Employee entity) => _db.Employees.Add(entity);

    public void Update(Employee entity) => _db.Employees.Update(entity);

    public void Remove(Employee entity) => _db.Employees.Remove(entity);
}
