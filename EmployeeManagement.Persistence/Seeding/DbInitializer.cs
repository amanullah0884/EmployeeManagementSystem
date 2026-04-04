using EmployeeManagement.Application.Abstractions;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Seeding;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher, CancellationToken cancellationToken = default)
    {
        if (await context.Permissions.AnyAsync(cancellationToken))
            return;

        foreach (var name in AppPermissions.All)
            context.Permissions.Add(new Permission { Name = name });

        await context.SaveChangesAsync(cancellationToken);

        var permissions = await context.Permissions.ToListAsync(cancellationToken);
        var byName = permissions.ToDictionary(p => p.Name, StringComparer.Ordinal);

        Permission[] Pick(params string[] names) =>
            names.Select(n => byName[n]).ToArray();

        var roles = new List<Role>
        {
            new()
            {
                Name = "Admin",
                Permissions = Pick(AppPermissions.All.ToArray())
            },
            new()
            {
                Name = "HR",
                Permissions = Pick(
                    AppPermissions.EmployeesRead, AppPermissions.EmployeesWrite, AppPermissions.EmployeesDelete,
                    AppPermissions.DepartmentsRead, AppPermissions.DepartmentsWrite, AppPermissions.DepartmentsDelete,
                    AppPermissions.DesignationsRead, AppPermissions.DesignationsWrite, AppPermissions.DesignationsDelete,
                    AppPermissions.SalariesRead, AppPermissions.SalariesDisburse,
                    AppPermissions.ReportsRead)
            },
            new()
            {
                Name = "Accountant",
                Permissions = Pick(
                    AppPermissions.SalariesRead, AppPermissions.SalariesDisburse,
                    AppPermissions.ReportsRead)
            },
            new()
            {
                Name = "Viewer",
                Permissions = Pick(
                    AppPermissions.EmployeesRead,
                    AppPermissions.DepartmentsRead,
                    AppPermissions.DesignationsRead,
                    AppPermissions.SalariesRead,
                    AppPermissions.ReportsRead)
            }
        };

        context.Roles.AddRange(roles);
        await context.SaveChangesAsync(cancellationToken);

        var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin", cancellationToken);

        context.Users.Add(new User
        {
            Username = "admin",
            PasswordHash = passwordHasher.Hash("Admin@123"),
            RoleId = adminRole.Id
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}
