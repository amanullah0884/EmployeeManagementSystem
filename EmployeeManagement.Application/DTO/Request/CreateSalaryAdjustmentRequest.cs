namespace EmployeeManagement.Application.DTO.Request;

public record CreateSalaryAdjustmentRequest(
    int EmployeeId,
    int Year,
    int Month,
    decimal Amount,
    string Reason);
