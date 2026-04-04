using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Abstractions.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdWithPermissionsAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Role>> ListAsync(CancellationToken cancellationToken = default);
}
