using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Mappers;

public static class SalaryMapper
{
    public static SalaryResponse ToResponse(Salary salary) =>
        new(
            salary.Id,
            salary.EmployeeId,
            salary.Employee?.Name ?? string.Empty,
            salary.Amount,
            salary.BaseComponent,
            salary.AdjustmentComponent,
            salary.Month,
            salary.Year,
            salary.PaidDate,
            salary.Reference);

    public static SalaryResponse ToResponse(Salary salary, string employeeName) =>
        new(
            salary.Id,
            salary.EmployeeId,
            employeeName,
            salary.Amount,
            salary.BaseComponent,
            salary.AdjustmentComponent,
            salary.Month,
            salary.Year,
            salary.PaidDate,
            salary.Reference);
}
