using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Abstractions.Repositories;

public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Department>> ListAsync(CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken cancellationToken = default);

    void Add(Department entity);

    void Update(Department entity);

    void Remove(Department entity);
}
