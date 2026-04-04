namespace EmployeeManagement.Application.DTO.Response;

public record AuditLogResponse(
    long Id,
    string EventCategory,
    int? UserId,
    string? Username,
    string Action,
    string EntityName,
    string? EntityId,
    string? Details,
    string? CorrelationId,
    DateTime CreatedAtUtc);
