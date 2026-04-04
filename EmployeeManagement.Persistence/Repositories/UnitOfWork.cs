using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Persistence.DbContext;

namespace EmployeeManagement.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private IEmployeeRepository? _employees;
    private IDepartmentRepository? _departments;
    private IDesignationRepository? _designations;
    private ISalaryRepository? _salaries;
    private IUserRepository? _users;
    private IRoleRepository? _roles;
    private IPermissionRepository? _permissions;
    private IAuditLogRepository? _auditLogs;
    private ISalaryAdjustmentRepository? _salaryAdjustments;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
    }

    public IEmployeeRepository Employees => _employees ??= new EmployeeRepository(_db);

    public IDepartmentRepository Departments => _departments ??= new DepartmentRepository(_db);

    public IDesignationRepository Designations => _designations ??= new DesignationRepository(_db);

    public ISalaryRepository Salaries => _salaries ??= new SalaryRepository(_db);

    public ISalaryAdjustmentRepository SalaryAdjustments => _salaryAdjustments ??= new SalaryAdjustmentRepository(_db);

    public IUserRepository Users => _users ??= new UserRepository(_db);

    public IRoleRepository Roles => _roles ??= new RoleRepository(_db);

    public IPermissionRepository Permissions => _permissions ??= new PermissionRepository(_db);

    public IAuditLogRepository AuditLogs => _auditLogs ??= new AuditLogRepository(_db);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _db.SaveChangesAsync(cancellationToken);
}
