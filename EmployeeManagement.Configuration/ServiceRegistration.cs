using EmployeeManagement.Application;
using EmployeeManagement.Application.Abstractions;
using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Infrastructure;
using EmployeeManagement.Persistence.Audit;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Logging;
using EmployeeManagement.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Configuration;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuditTrailWriter, AuditTrailWriter>();
        services.AddScoped<IAuthenticationActivityLogger, AuthenticationActivityLogger>();

        return services;
    }

    public static IServiceCollection AddEmployeeManagementServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddApplication();
        services.AddJwtInfrastructure(configuration);
        return services;
    }
}
