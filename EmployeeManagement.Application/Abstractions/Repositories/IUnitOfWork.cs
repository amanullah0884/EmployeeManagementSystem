namespace EmployeeManagement.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    IEmployeeRepository Employees { get; }

    IDepartmentRepository Departments { get; }

    IDesignationRepository Designations { get; }

    ISalaryRepository Salaries { get; }

    ISalaryAdjustmentRepository SalaryAdjustments { get; }

    IUserRepository Users { get; }

    IRoleRepository Roles { get; }

    IPermissionRepository Permissions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
