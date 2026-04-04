using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;
using EmployeeManagement.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Authorize(Policy = AppPermissions.UsersManage)]
[Route("api/[controller]")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogsController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecent([FromQuery] int take = 100, CancellationToken cancellationToken = default)
    {
        var result = await _auditLogService.GetRecentAsync(take, cancellationToken);
        return this.ToStandardOk(result, "Audit logs retrieved successfully");
    }
}
