using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;
using EmployeeManagement.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SalariesController : ControllerBase
{
    private readonly ISalaryService _salaryService;

    public SalariesController(ISalaryService salaryService)
    {
        _salaryService = salaryService;
    }

    [HttpGet("employee/{employeeId:int}")]
    [Authorize(Policy = AppPermissions.SalariesRead)]
    public async Task<IActionResult> GetByEmployee(int employeeId, CancellationToken cancellationToken)
    {
        var result = await _salaryService.GetByEmployeeAsync(employeeId, cancellationToken);
        return this.ToStandardOk(result, "Salary records retrieved successfully");
    }

    [HttpGet("period/{year:int}/{month:int}")]
    [Authorize(Policy = AppPermissions.SalariesRead)]
    public async Task<IActionResult> GetByPeriod(int year, int month, CancellationToken cancellationToken)
    {
        var result = await _salaryService.GetByPeriodAsync(year, month, cancellationToken);
        return this.ToStandardOk(result, "Salary records retrieved successfully");
    }

    [HttpGet("adjustments/employee/{employeeId:int}")]
    [Authorize(Policy = AppPermissions.SalariesRead)]
    public async Task<IActionResult> GetAdjustments(int employeeId, CancellationToken cancellationToken)
    {
        var result = await _salaryService.GetAdjustmentsByEmployeeAsync(employeeId, cancellationToken);
        return this.ToStandardOk(result, "Salary adjustments retrieved successfully");
    }

    [HttpPost("adjustments")]
    [Authorize(Policy = AppPermissions.SalariesDisburse)]
    public async Task<IActionResult> AddAdjustment([FromBody] CreateSalaryAdjustmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _salaryService.AddAdjustmentAsync(request, cancellationToken);
        return this.ToStandardOk(result, "Salary adjustment recorded successfully");
    }

    [HttpPost("disburse")]
    [Authorize(Policy = AppPermissions.SalariesDisburse)]
    public async Task<IActionResult> Disburse([FromBody] DisburseSalaryRequest request, CancellationToken cancellationToken)
    {
        var result = await _salaryService.DisburseAsync(request, cancellationToken);
        return this.ToStandardOk(result, "Salary disbursement completed successfully");
    }
}
