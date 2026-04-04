using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Application.Mappers;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Implementations;

public class AuditLogService : IAuditLogService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuditLogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<IReadOnlyList<AuditLogResponse>>> GetRecentAsync(int take = 100, CancellationToken cancellationToken = default)
    {
        take = Math.Clamp(take, 1, 500);
        var logs = await _unitOfWork.AuditLogs.ListRecentAsync(take, cancellationToken);
        var dto = logs.Select(AuditLogMapper.ToResponse).ToList();

        return ApiResult<IReadOnlyList<AuditLogResponse>>.Ok(dto);
    }
}
