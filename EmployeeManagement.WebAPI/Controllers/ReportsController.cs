using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;
using EmployeeManagement.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("headcount-by-department")]
    [Authorize(Policy = AppPermissions.ReportsRead)]
    public async Task<IActionResult> HeadcountByDepartment(CancellationToken cancellationToken)
    {
        var result = await _reportService.HeadcountByDepartmentAsync(cancellationToken);
        return this.ToStandardOk(result, "Headcount by department retrieved successfully");
    }

    [HttpGet("total-salary-disbursed/{year:int}/{month:int}")]
    [Authorize(Policy = AppPermissions.ReportsRead)]
    public async Task<IActionResult> TotalSalaryDisbursed(int year, int month, CancellationToken cancellationToken)
    {
        var result = await _reportService.TotalSalaryDisbursedAsync(year, month, cancellationToken);
        return this.ToStandardOk(result, "Total salary disbursed for period retrieved successfully");
    }

    [HttpGet("salary-distribution-by-department/{year:int}/{month:int}")]
    [Authorize(Policy = AppPermissions.ReportsRead)]
    public async Task<IActionResult> SalaryDistributionByDepartment(int year, int month, CancellationToken cancellationToken)
    {
        var result = await _reportService.SalaryDistributionByDepartmentAsync(year, month, cancellationToken);
        return this.ToStandardOk(result, "Salary distribution by department retrieved successfully");
    }

    [HttpGet("monthly-salary-summary/{year:int}/{month:int}")]
    [Authorize(Policy = AppPermissions.ReportsRead)]
    public async Task<IActionResult> MonthlySalarySummary(int year, int month, CancellationToken cancellationToken)
    {
        var result = await _reportService.MonthlySalarySummaryAsync(year, month, cancellationToken);
        return this.ToStandardOk(result, "Monthly salary summary retrieved successfully");
    }

    [HttpGet("employee-salary-history/{employeeId:int}")]
    [Authorize(Policy = AppPermissions.ReportsRead)]
    public async Task<IActionResult> EmployeeSalaryHistory(int employeeId, CancellationToken cancellationToken)
    {
        var result = await _reportService.EmployeeSalaryHistoryAsync(employeeId, cancellationToken);
        return this.ToStandardOk(result, "Employee salary history retrieved successfully");
    }

    [HttpGet("employee-directory")]
    [Authorize(Policy = AppPermissions.ReportsRead)]
    public async Task<IActionResult> EmployeeDirectory(CancellationToken cancellationToken)
    {
        var result = await _reportService.EmployeeDirectoryAsync(cancellationToken);
        return this.ToStandardOk(result, "Employee directory retrieved successfully");
    }
}
