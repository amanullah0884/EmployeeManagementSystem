namespace EmployeeManagement.Application.DTO.Request;

public record UpdateEmployeeRequest(
    string Name,
    string Email,
    string Phone,
    int DepartmentId,
    int DesignationId,
    bool IsActive,
    decimal MonthlyBaseSalary);
