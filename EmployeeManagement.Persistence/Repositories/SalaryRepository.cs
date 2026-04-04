using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories;

public class SalaryRepository : ISalaryRepository
{
    private readonly AppDbContext _db;

    public SalaryRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Salary?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Salaries
            .Include(s => s.Employee)!.ThenInclude(e => e!.Department)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Salary>> ListByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        return await _db.Salaries
            .AsNoTracking()
            .Include(s => s.Employee)
            .Where(s => s.EmployeeId == employeeId)
            .OrderByDescending(s => s.Year)
            .ThenByDescending(s => s.Month)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Salary>> ListByPeriodAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        return await _db.Salaries
            .AsNoTracking()
            .Include(s => s.Employee)!.ThenInclude(e => e!.Department)
            .Where(s => s.Year == year && s.Month == month)
            .OrderBy(s => s.Employee!.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsForEmployeePeriodAsync(int employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        return await _db.Salaries.AnyAsync(
            s => s.EmployeeId == employeeId && s.Year == year && s.Month == month,
            cancellationToken);
    }

    public void Add(Salary entity) => _db.Salaries.Add(entity);

    public void AddRange(IEnumerable<Salary> entities) => _db.Salaries.AddRange(entities);
}
