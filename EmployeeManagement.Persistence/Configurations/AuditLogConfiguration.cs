using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.Property(x => x.EventCategory).HasMaxLength(64);
        builder.Property(x => x.Action).HasMaxLength(512);
        builder.Property(x => x.EntityName).HasMaxLength(128);
        builder.Property(x => x.CorrelationId).HasMaxLength(128);
        builder.HasIndex(x => x.CreatedAtUtc);
        builder.HasIndex(x => x.EventCategory);
    }
}
