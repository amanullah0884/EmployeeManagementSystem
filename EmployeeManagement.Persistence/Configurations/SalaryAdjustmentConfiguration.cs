using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Persistence.Configurations;

public class SalaryAdjustmentConfiguration : IEntityTypeConfiguration<SalaryAdjustment>
{
    public void Configure(EntityTypeBuilder<SalaryAdjustment> builder)
    {
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Reason).HasMaxLength(500);

        builder.HasIndex(x => new { x.EmployeeId, x.Year, x.Month });

        builder.HasOne(x => x.Employee)
            .WithMany(e => e.SalaryAdjustments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
