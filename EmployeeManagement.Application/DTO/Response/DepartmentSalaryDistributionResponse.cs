namespace EmployeeManagement.Application.DTO.Response;

public record DepartmentSalaryDistributionResponse(
    int DepartmentId,
    string DepartmentName,
    decimal TotalSalary,
    int EmployeeCount);
