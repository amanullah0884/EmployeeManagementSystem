using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Mappers;

public static class EmployeeMapper
{
    public static EmployeeResponse ToResponse(Employee employee) =>
        new(
            employee.Id,
            employee.Name,
            employee.Email,
            employee.Phone,
            employee.DepartmentId,
            employee.DesignationId,
            employee.Department.Name,
            employee.Designation.Name,
            employee.IsActive,
            employee.MonthlyBaseSalary,
            employee.CreatedAt);
}
