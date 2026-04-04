using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IReportService
{
    Task<ApiResult<IReadOnlyList<DepartmentHeadcountResponse>>> HeadcountByDepartmentAsync(CancellationToken cancellationToken = default);

    Task<ApiResult<PayrollSummaryResponse>> TotalSalaryDisbursedAsync(int year, int month, CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<DepartmentSalaryDistributionResponse>>> SalaryDistributionByDepartmentAsync(int year, int month, CancellationToken cancellationToken = default);

    Task<ApiResult<MonthlySalarySummaryResponse>> MonthlySalarySummaryAsync(int year, int month, CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<EmployeeSalaryHistoryRowResponse>>> EmployeeSalaryHistoryAsync(int employeeId, CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<EmployeeDirectoryEntryResponse>>> EmployeeDirectoryAsync(CancellationToken cancellationToken = default);
}
