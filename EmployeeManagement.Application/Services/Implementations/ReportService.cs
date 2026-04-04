using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Application.Reporting;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Implementations;

/// <summary>
/// Application façade over <see cref="IReportQueryService"/> (aggregated reporting separated from CRUD services).
/// </summary>
public class ReportService : IReportService
{
    private readonly IReportQueryService _reportQueries;

    public ReportService(IReportQueryService reportQueries)
    {
        _reportQueries = reportQueries;
    }

    public async Task<ApiResult<IReadOnlyList<DepartmentHeadcountResponse>>> HeadcountByDepartmentAsync(CancellationToken cancellationToken = default)
    {
        var data = await _reportQueries.HeadcountByDepartmentAsync(cancellationToken);
        return ApiResult<IReadOnlyList<DepartmentHeadcountResponse>>.Ok(data);
    }

    public async Task<ApiResult<PayrollSummaryResponse>> TotalSalaryDisbursedAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        if (month is < 1 or > 12)
            return ApiResult<PayrollSummaryResponse>.Fail("Month must be between 1 and 12.");

        var data = await _reportQueries.TotalSalaryDisbursedForPeriodAsync(year, month, cancellationToken);
        return ApiResult<PayrollSummaryResponse>.Ok(data);
    }

    public async Task<ApiResult<IReadOnlyList<DepartmentSalaryDistributionResponse>>> SalaryDistributionByDepartmentAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        if (month is < 1 or > 12)
            return ApiResult<IReadOnlyList<DepartmentSalaryDistributionResponse>>.Fail("Month must be between 1 and 12.");

        var data = await _reportQueries.SalaryDistributionByDepartmentAsync(year, month, cancellationToken);
        return ApiResult<IReadOnlyList<DepartmentSalaryDistributionResponse>>.Ok(data);
    }

    public async Task<ApiResult<MonthlySalarySummaryResponse>> MonthlySalarySummaryAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        if (month is < 1 or > 12)
            return ApiResult<MonthlySalarySummaryResponse>.Fail("Month must be between 1 and 12.");

        var data = await _reportQueries.MonthlySalarySummaryAsync(year, month, cancellationToken);
        return ApiResult<MonthlySalarySummaryResponse>.Ok(data);
    }

    public async Task<ApiResult<IReadOnlyList<EmployeeSalaryHistoryRowResponse>>> EmployeeSalaryHistoryAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        var data = await _reportQueries.EmployeeSalaryHistoryAsync(employeeId, cancellationToken);
        return ApiResult<IReadOnlyList<EmployeeSalaryHistoryRowResponse>>.Ok(data);
    }

    public async Task<ApiResult<IReadOnlyList<EmployeeDirectoryEntryResponse>>> EmployeeDirectoryAsync(CancellationToken cancellationToken = default)
    {
        var data = await _reportQueries.EmployeeDirectoryAsync(cancellationToken);
        return ApiResult<IReadOnlyList<EmployeeDirectoryEntryResponse>>.Ok(data);
    }
}
