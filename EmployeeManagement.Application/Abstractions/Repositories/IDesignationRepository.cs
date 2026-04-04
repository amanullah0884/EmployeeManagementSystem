using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Abstractions.Repositories;

public interface IDesignationRepository
{
    Task<Designation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Designation>> ListAsync(CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken cancellationToken = default);

    void Add(Designation entity);

    void Update(Designation entity);

    void Remove(Designation entity);
}
