using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.SalaryCalculation;

/// <summary>
/// Application-layer salary calculation (base + monthly adjustments).
/// </summary>
public interface ISalaryCalculationService
{
    decimal CalculateNetMonthlyPay(Employee employee, int year, int month, IReadOnlyList<SalaryAdjustment> adjustmentsForPeriod);
}
