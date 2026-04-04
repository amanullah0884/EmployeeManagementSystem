using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Abstractions.Repositories;

public interface ISalaryAdjustmentRepository
{
    Task<IReadOnlyList<SalaryAdjustment>> ListForEmployeePeriodAsync(int employeeId, int year, int month, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SalaryAdjustment>> ListForEmployeeAsync(int employeeId, CancellationToken cancellationToken = default);

    void Add(SalaryAdjustment entity);
}
