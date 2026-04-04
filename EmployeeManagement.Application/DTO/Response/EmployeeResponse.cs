namespace EmployeeManagement.Application.DTO.Response;

public record EmployeeResponse(
    int Id,
    string Name,
    string Email,
    string Phone,
    int DepartmentId,
    int DesignationId,
    string DepartmentName,
    string DesignationName,
    bool IsActive,
    decimal MonthlyBaseSalary,
    DateTime CreatedAt);
