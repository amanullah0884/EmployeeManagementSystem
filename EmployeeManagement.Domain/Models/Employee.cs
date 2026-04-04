namespace EmployeeManagement.Domain.Models;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int DesignationId { get; set; }

    public bool IsActive { get; set; }

    /// <summary>Monthly base pay used by application-layer salary calculation.</summary>
    public decimal MonthlyBaseSalary { get; set; }

    public DateTime CreatedAt { get; set; }

    public Department Department { get; set; } = null!;

    public Designation Designation { get; set; } = null!;

    public ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public ICollection<SalaryAdjustment> SalaryAdjustments { get; set; } = new List<SalaryAdjustment>();
}
