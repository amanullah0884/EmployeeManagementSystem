namespace EmployeeManagement.Application.DTO.Response;

public record SalaryResponse(
    int Id,
    int EmployeeId,
    string EmployeeName,
    decimal Amount,
    decimal BaseComponent,
    decimal AdjustmentComponent,
    int Month,
    int Year,
    DateTime PaidDate,
    string? Reference);
