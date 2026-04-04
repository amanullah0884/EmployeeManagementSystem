namespace EmployeeManagement.Application.DTO.Response;

public record EmployeeSalaryHistoryRowResponse(
    int Year,
    int Month,
    decimal NetPaid,
    DateTime PaidDate,
    string? Reference);
