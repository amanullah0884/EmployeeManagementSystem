using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Abstractions.Repositories;

public interface ISalaryRepository
{
    Task<Salary?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Salary>> ListByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Salary>> ListByPeriodAsync(int year, int month, CancellationToken cancellationToken = default);

    Task<bool> ExistsForEmployeePeriodAsync(int employeeId, int year, int month, CancellationToken cancellationToken = default);

    void Add(Salary entity);

    void AddRange(IEnumerable<Salary> entities);
}
