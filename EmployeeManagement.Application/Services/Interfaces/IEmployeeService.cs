using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<ApiResult<IReadOnlyList<EmployeeResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ApiResult<EmployeeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ApiResult<EmployeeResponse>> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult<EmployeeResponse>> UpdateAsync(int id, UpdateEmployeeRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult<EmployeeResponse>> DeactivateAsync(int id, CancellationToken cancellationToken = default);

    Task<ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
