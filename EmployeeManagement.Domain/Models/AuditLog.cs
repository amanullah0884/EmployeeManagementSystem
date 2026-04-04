namespace EmployeeManagement.Domain.Models;

public class AuditLog
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public string? Username { get; set; }

    /// <summary>Authentication, Http, DataChange, System, Error.</summary>
    public string EventCategory { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string EntityName { get; set; } = null!;

    public string? EntityId { get; set; }

    public string? Details { get; set; }

    /// <summary>Optional correlation (e.g. request trace identifier).</summary>
    public string? CorrelationId { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
