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
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    [Authorize(Policy = AppPermissions.DepartmentsRead)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetAllAsync(cancellationToken);
        return this.ToStandardOk(result, "Departments retrieved successfully");
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = AppPermissions.DepartmentsRead)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetByIdAsync(id, cancellationToken);
        return this.ToStandardNotFound(result, "Department retrieved successfully");
    }

    [HttpPost]
    [Authorize(Policy = AppPermissions.DepartmentsWrite)]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _departmentService.CreateAsync(request, cancellationToken);
        return this.ToStandardCreated(nameof(GetById), new { id = result.Data?.Id }, result, "Department created successfully");
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = AppPermissions.DepartmentsWrite)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _departmentService.UpdateAsync(id, request, cancellationToken);
        return this.ToStandardOk(result, "Department updated successfully");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = AppPermissions.DepartmentsDelete)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _departmentService.DeleteAsync(id, cancellationToken);
        return this.ToStandardDelete(result, "Department deleted successfully");
    }
}
