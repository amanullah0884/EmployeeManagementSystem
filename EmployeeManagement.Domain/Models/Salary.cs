namespace EmployeeManagement.Domain.Models;

public class Salary
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    /// <summary>Net amount paid (base + adjustments for the period).</summary>
    public decimal Amount { get; set; }

    public decimal BaseComponent { get; set; }

    public decimal AdjustmentComponent { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public DateTime PaidDate { get; set; }

    public string? Reference { get; set; }

    public Employee Employee { get; set; } = null!;
}
