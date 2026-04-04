using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Mappers;

public static class DepartmentMapper
{
    public static DepartmentResponse ToResponse(Department department) =>
        new(department.Id, department.Name, department.Description, department.Employees.Count);
}
