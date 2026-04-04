using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories;

public class DesignationRepository : IDesignationRepository
{
    private readonly AppDbContext _db;

    public DesignationRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Designation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Designations
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Designation>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Designations
            .AsNoTracking()
            .Include(d => d.Employees)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken cancellationToken = default)
    {
        var q = _db.Designations.Where(d => d.Name == name);
        if (excludeId is { } id)
            q = q.Where(d => d.Id != id);

        return await q.AnyAsync(cancellationToken);
    }

    public void Add(Designation entity) => _db.Designations.Add(entity);

    public void Update(Designation entity) => _db.Designations.Update(entity);

    public void Remove(Designation entity) => _db.Designations.Remove(entity);
}
