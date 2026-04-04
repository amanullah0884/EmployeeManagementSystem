using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories;

public class SalaryAdjustmentRepository : ISalaryAdjustmentRepository
{
    private readonly AppDbContext _db;

    public SalaryAdjustmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<SalaryAdjustment>> ListForEmployeePeriodAsync(
        int employeeId,
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        return await _db.SalaryAdjustments
            .AsNoTracking()
            .Where(a => a.EmployeeId == employeeId && a.Year == year && a.Month == month)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SalaryAdjustment>> ListForEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        return await _db.SalaryAdjustments
            .AsNoTracking()
            .Include(a => a.Employee)
            .Where(a => a.EmployeeId == employeeId)
            .OrderByDescending(a => a.Year)
            .ThenByDescending(a => a.Month)
            .ThenByDescending(a => a.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public void Add(SalaryAdjustment entity) => _db.SalaryAdjustments.Add(entity);
}
