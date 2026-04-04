using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<ApiResult<IReadOnlyList<DepartmentResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ApiResult<DepartmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ApiResult<DepartmentResponse>> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult<DepartmentResponse>> UpdateAsync(int id, UpdateDepartmentRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
