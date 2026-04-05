using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.DbContext;

/// <summary>
/// PostgreSQL context for application logs and audit trail (separate from main MSSQL data).
/// </summary>
public class LoggingDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public LoggingDbContext(DbContextOptions<LoggingDbContext> options)
        : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(LoggingDbContext).Assembly,
            t => t == typeof(AuditLogConfiguration));
    }
}
