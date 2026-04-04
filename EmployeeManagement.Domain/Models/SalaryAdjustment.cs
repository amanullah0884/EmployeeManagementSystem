namespace EmployeeManagement.Domain.Models;

/// <summary>
/// Monthly salary adjustment (bonus, deduction, correction) applied before disbursement calculation.
/// </summary>
public class SalaryAdjustment
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public decimal Amount { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public Employee Employee { get; set; } = null!;
}
