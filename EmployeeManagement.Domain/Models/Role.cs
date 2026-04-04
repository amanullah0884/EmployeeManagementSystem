namespace EmployeeManagement.Domain.Models;

public class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public ICollection<User> Users { get; set; } = new List<User>();
}
