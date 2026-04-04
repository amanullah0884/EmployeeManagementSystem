using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Mappers;

public static class DesignationMapper
{
    public static DesignationResponse ToResponse(Designation designation) =>
        new(designation.Id, designation.Name, designation.Employees.Count);
}
