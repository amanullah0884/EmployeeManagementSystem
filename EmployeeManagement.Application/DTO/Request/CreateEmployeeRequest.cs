namespace EmployeeManagement.Application.DTO.Request;

public record CreateEmployeeRequest(
    string Name,
    string Email,
    string Phone,
    int DepartmentId,
    int DesignationId,
    decimal MonthlyBaseSalary);
