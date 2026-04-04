namespace EmployeeManagement.Application.DTO.Request;

/// <summary>
/// Disburses calculated net pay (monthly base + adjustments) for each employee for the given period.
/// </summary>
public record DisburseSalaryRequest(
    IReadOnlyList<int> EmployeeIds,
    int Year,
    int Month,
    DateTime PaidDate,
    string? Reference);
