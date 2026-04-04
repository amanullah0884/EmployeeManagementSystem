using EmployeeManagement.Application.DTO.Response;

namespace EmployeeManagement.Application.Reporting;

/// <summary>
/// Read-only reporting queries separated from core CRUD services.
/// </summary>
public interface IReportQueryService
{
    Task<IReadOnlyList<DepartmentHeadcountResponse>> HeadcountByDepartmentAsync(CancellationToken cancellationToken = default);

    Task<PayrollSummaryResponse> TotalSalaryDisbursedForPeriodAsync(int year, int month, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DepartmentSalaryDistributionResponse>> SalaryDistributionByDepartmentAsync(int year, int month, CancellationToken cancellationToken = default);

    Task<MonthlySalarySummaryResponse> MonthlySalarySummaryAsync(int year, int month, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EmployeeSalaryHistoryRowResponse>> EmployeeSalaryHistoryAsync(int employeeId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EmployeeDirectoryEntryResponse>> EmployeeDirectoryAsync(CancellationToken cancellationToken = default);
}
