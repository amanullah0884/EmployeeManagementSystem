using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Mappers;

public static class AuditLogMapper
{
    public static AuditLogResponse ToResponse(AuditLog log) =>
        new(
            log.Id,
            log.EventCategory,
            log.UserId,
            log.Username,
            log.Action,
            log.EntityName,
            log.EntityId,
            log.Details,
            log.CorrelationId,
            log.CreatedAtUtc);
}
