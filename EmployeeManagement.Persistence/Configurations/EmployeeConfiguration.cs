using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.MonthlyBaseSalary).HasPrecision(18, 2);

        builder.HasOne(x => x.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Designation)
            .WithMany(d => d.Employees)
            .HasForeignKey(x => x.DesignationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
