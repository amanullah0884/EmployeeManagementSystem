using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Authorize(Policy = AppPermissions.UsersManage)]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public RolesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.Roles.ListAsync(cancellationToken);
        var data = roles.Select(r => new { r.Id, r.Name }).ToList();
        return Ok(StandardResponseFactory.Success(data, "Roles retrieved successfully", "200"));
    }
}
