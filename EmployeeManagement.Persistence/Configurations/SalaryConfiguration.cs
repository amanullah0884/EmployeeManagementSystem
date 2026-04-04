using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Persistence.Configurations;

public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
{
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.Property(s => s.Amount).HasPrecision(18, 2);
        builder.Property(s => s.BaseComponent).HasPrecision(18, 2);
        builder.Property(s => s.AdjustmentComponent).HasPrecision(18, 2);
        builder.HasIndex(s => new { s.EmployeeId, s.Year, s.Month }).IsUnique();

        builder.HasOne(s => s.Employee)
            .WithMany(emp => emp.Salaries)
            .HasForeignKey(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
