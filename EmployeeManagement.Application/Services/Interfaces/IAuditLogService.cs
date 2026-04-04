using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IAuditLogService
{
    Task<ApiResult<IReadOnlyList<AuditLogResponse>>> GetRecentAsync(int take = 100, CancellationToken cancellationToken = default);
}
