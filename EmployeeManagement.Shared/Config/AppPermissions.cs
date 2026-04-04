namespace EmployeeManagement.Shared;

/// <summary>
/// Permission claim values used for RBAC policies and JWT claims.
/// </summary>
public static class AppPermissions
{
    public const string EmployeesRead = "employees.read";
    public const string EmployeesWrite = "employees.write";
    public const string EmployeesDelete = "employees.delete";

    public const string DepartmentsRead = "departments.read";
    public const string DepartmentsWrite = "departments.write";
    public const string DepartmentsDelete = "departments.delete";

    public const string DesignationsRead = "designations.read";
    public const string DesignationsWrite = "designations.write";
    public const string DesignationsDelete = "designations.delete";

    public const string SalariesRead = "salaries.read";
    public const string SalariesDisburse = "salaries.disburse";

    public const string ReportsRead = "reports.read";

    public const string UsersManage = "users.manage";

    public static IReadOnlyList<string> All { get; } =
    [
        EmployeesRead, EmployeesWrite, EmployeesDelete,
        DepartmentsRead, DepartmentsWrite, DepartmentsDelete,
        DesignationsRead, DesignationsWrite, DesignationsDelete,
        SalariesRead, SalariesDisburse,
        ReportsRead,
        UsersManage
    ];
}
