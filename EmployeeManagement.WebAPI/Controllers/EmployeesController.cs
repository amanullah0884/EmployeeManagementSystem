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
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [Authorize(Policy = AppPermissions.EmployeesRead)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetAllAsync(cancellationToken);
        return this.ToStandardOk(result, "Employees retrieved successfully");
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = AppPermissions.EmployeesRead)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetByIdAsync(id, cancellationToken);
        return this.ToStandardNotFound(result, "Employee retrieved successfully");
    }

    [HttpPost]
    [Authorize(Policy = AppPermissions.EmployeesWrite)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var result = await _employeeService.CreateAsync(request, cancellationToken);
        return this.ToStandardCreated(nameof(GetById), new { id = result.Data?.Id }, result, "Employee created successfully");
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = AppPermissions.EmployeesWrite)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var result = await _employeeService.UpdateAsync(id, request, cancellationToken);
        return this.ToStandardOk(result, "Employee updated successfully");
    }

    [HttpPatch("{id:int}/deactivate")]
    [Authorize(Policy = AppPermissions.EmployeesWrite)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
    {
        var result = await _employeeService.DeactivateAsync(id, cancellationToken);
        return this.ToStandardOk(result, "Employee deactivated successfully");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = AppPermissions.EmployeesDelete)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _employeeService.DeleteAsync(id, cancellationToken);
        return this.ToStandardDelete(result, "Employee deleted successfully");
    }
}
