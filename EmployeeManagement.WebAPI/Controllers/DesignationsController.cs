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
public class DesignationsController : ControllerBase
{
    private readonly IDesignationService _designationService;

    public DesignationsController(IDesignationService designationService)
    {
        _designationService = designationService;
    }

    [HttpGet]
    [Authorize(Policy = AppPermissions.DesignationsRead)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _designationService.GetAllAsync(cancellationToken);
        return this.ToStandardOk(result, "Designations retrieved successfully");
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = AppPermissions.DesignationsRead)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _designationService.GetByIdAsync(id, cancellationToken);
        return this.ToStandardNotFound(result, "Designation retrieved successfully");
    }

    [HttpPost]
    [Authorize(Policy = AppPermissions.DesignationsWrite)]
    public async Task<IActionResult> Create([FromBody] CreateDesignationRequest request, CancellationToken cancellationToken)
    {
        var result = await _designationService.CreateAsync(request, cancellationToken);
        return this.ToStandardCreated(nameof(GetById), new { id = result.Data?.Id }, result, "Designation created successfully");
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = AppPermissions.DesignationsWrite)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDesignationRequest request, CancellationToken cancellationToken)
    {
        var result = await _designationService.UpdateAsync(id, request, cancellationToken);
        return this.ToStandardOk(result, "Designation updated successfully");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = AppPermissions.DesignationsDelete)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _designationService.DeleteAsync(id, cancellationToken);
        return this.ToStandardDelete(result, "Designation deleted successfully");
    }
}
