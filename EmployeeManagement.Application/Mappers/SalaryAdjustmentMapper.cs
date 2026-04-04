using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Mappers;

public static class SalaryAdjustmentMapper
{
    public static SalaryAdjustmentResponse ToResponse(SalaryAdjustment adjustment) =>
        new(
            adjustment.Id,
            adjustment.EmployeeId,
            adjustment.Employee?.Name ?? string.Empty,
            adjustment.Year,
            adjustment.Month,
            adjustment.Amount,
            adjustment.Reason,
            adjustment.CreatedAtUtc);
}
