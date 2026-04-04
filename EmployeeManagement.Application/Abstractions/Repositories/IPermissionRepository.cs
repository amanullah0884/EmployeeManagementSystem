using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Abstractions.Repositories;

public interface IPermissionRepository
{
    Task<IReadOnlyList<Permission>> ListAsync(CancellationToken cancellationToken = default);
}
