using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Abstractions.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Employee>> ListAsync(CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(string email, int? excludeId, CancellationToken cancellationToken = default);

    void Add(Employee entity);

    void Update(Employee entity);

    void Remove(Employee entity);
}
