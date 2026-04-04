using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface ISalaryService
{
    Task<ApiResult<IReadOnlyList<SalaryResponse>>> GetByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<SalaryResponse>>> GetByPeriodAsync(int year, int month, CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<SalaryResponse>>> DisburseAsync(DisburseSalaryRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult<SalaryAdjustmentResponse>> AddAdjustmentAsync(CreateSalaryAdjustmentRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<SalaryAdjustmentResponse>>> GetAdjustmentsByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default);
}
