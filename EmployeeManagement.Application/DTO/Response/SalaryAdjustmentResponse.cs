namespace EmployeeManagement.Application.DTO.Response;

public record SalaryAdjustmentResponse(
    int Id,
    int EmployeeId,
    string EmployeeName,
    int Year,
    int Month,
    decimal Amount,
    string Reason,
    DateTime CreatedAtUtc);
