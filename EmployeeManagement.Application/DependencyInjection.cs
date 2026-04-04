using System.Reflection;
using EmployeeManagement.Application.Reporting;
using EmployeeManagement.Application.SalaryCalculation;
using EmployeeManagement.Application.Services.Implementations;
using EmployeeManagement.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ISalaryCalculationService, SalaryCalculationService>();
        services.AddScoped<IReportQueryService, ReportQueryService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IDesignationService, DesignationService>();
        services.AddScoped<ISalaryService, SalaryService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        return services;
    }
}
