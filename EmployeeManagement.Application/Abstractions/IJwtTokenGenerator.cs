using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IReadOnlyCollection<string> permissionNames);
}
