namespace EmployeeManagement.Application.DTO.Response;

public record PayrollSummaryResponse(int Year, int Month, decimal TotalAmount, int PaymentCount);
