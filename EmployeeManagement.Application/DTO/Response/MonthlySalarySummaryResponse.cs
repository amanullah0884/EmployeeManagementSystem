namespace EmployeeManagement.Application.DTO.Response;

public record MonthlySalarySummaryResponse(
    int Year,
    int Month,
    decimal TotalDisbursed,
    int PaymentCount,
    decimal AveragePayment);
