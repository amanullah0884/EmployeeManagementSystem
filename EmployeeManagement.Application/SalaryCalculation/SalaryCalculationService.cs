using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.SalaryCalculation;

public class SalaryCalculationService : ISalaryCalculationService
{
    public decimal CalculateNetMonthlyPay(Employee employee, int year, int month, IReadOnlyList<SalaryAdjustment> adjustmentsForPeriod)
    {
        var adjustmentSum = adjustmentsForPeriod
            .Where(a => a.EmployeeId == employee.Id && a.Year == year && a.Month == month)
            .Sum(a => a.Amount);

        return employee.MonthlyBaseSalary + adjustmentSum;
    }
}
