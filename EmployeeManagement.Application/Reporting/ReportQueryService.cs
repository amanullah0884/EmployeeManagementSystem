using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Application.DTO.Response;

namespace EmployeeManagement.Application.Reporting;

public class ReportQueryService : IReportQueryService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportQueryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<DepartmentHeadcountResponse>> HeadcountByDepartmentAsync(CancellationToken cancellationToken = default)
    {
        var departments = await _unitOfWork.Departments.ListAsync(cancellationToken);
        return departments
            .Select(d => new DepartmentHeadcountResponse(d.Id, d.Name, d.Employees.Count(e => e.IsActive)))
            .OrderBy(d => d.DepartmentName)
            .ToList();
    }

    public async Task<PayrollSummaryResponse> TotalSalaryDisbursedForPeriodAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        var salaries = await _unitOfWork.Salaries.ListByPeriodAsync(year, month, cancellationToken);
        var total = salaries.Sum(s => s.Amount);
        return new PayrollSummaryResponse(year, month, total, salaries.Count);
    }

    public async Task<IReadOnlyList<DepartmentSalaryDistributionResponse>> SalaryDistributionByDepartmentAsync(
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        var salaries = await _unitOfWork.Salaries.ListByPeriodAsync(year, month, cancellationToken);
        var grouped = salaries
            .Where(s => s.Employee != null)
            .GroupBy(s => new { s.Employee!.DepartmentId, s.Employee.Department.Name })
            .Select(g => new DepartmentSalaryDistributionResponse(
                g.Key.DepartmentId,
                g.Key.Name,
                g.Sum(x => x.Amount),
                g.Select(x => x.EmployeeId).Distinct().Count()))
            .OrderBy(x => x.DepartmentName)
            .ToList();

        return grouped;
    }

    public async Task<MonthlySalarySummaryResponse> MonthlySalarySummaryAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        var salaries = await _unitOfWork.Salaries.ListByPeriodAsync(year, month, cancellationToken);
        var total = salaries.Sum(s => s.Amount);
        var count = salaries.Count;
        var avg = count == 0 ? 0 : total / count;
        return new MonthlySalarySummaryResponse(year, month, total, count, avg);
    }

    public async Task<IReadOnlyList<EmployeeSalaryHistoryRowResponse>> EmployeeSalaryHistoryAsync(
        int employeeId,
        CancellationToken cancellationToken = default)
    {
        var rows = await _unitOfWork.Salaries.ListByEmployeeAsync(employeeId, cancellationToken);
        return rows
            .OrderByDescending(s => s.Year)
            .ThenByDescending(s => s.Month)
            .Select(s => new EmployeeSalaryHistoryRowResponse(s.Year, s.Month, s.Amount, s.PaidDate, s.Reference))
            .ToList();
    }

    public async Task<IReadOnlyList<EmployeeDirectoryEntryResponse>> EmployeeDirectoryAsync(CancellationToken cancellationToken = default)
    {
        var employees = await _unitOfWork.Employees.ListAsync(cancellationToken);
        return employees
            .OrderBy(e => e.Name)
            .Select(e => new EmployeeDirectoryEntryResponse(
                e.Id,
                e.Name,
                e.Email,
                e.Department.Name,
                e.Designation.Name,
                e.IsActive))
            .ToList();
    }
}
