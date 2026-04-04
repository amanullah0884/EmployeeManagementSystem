using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IDesignationService
{
    Task<ApiResult<IReadOnlyList<DesignationResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ApiResult<DesignationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ApiResult<DesignationResponse>> CreateAsync(CreateDesignationRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult<DesignationResponse>> UpdateAsync(int id, UpdateDesignationRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
