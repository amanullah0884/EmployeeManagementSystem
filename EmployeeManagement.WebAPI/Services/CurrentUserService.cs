using System.Security.Claims;
using EmployeeManagement.Application.Abstractions;

namespace EmployeeManagement.WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var v = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(v, out var id) ? id : null;
        }
    }

    public string? Username =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)
        ?? _httpContextAccessor.HttpContext?.User.Identity?.Name;

    public IReadOnlyList<string> Permissions =>
        _httpContextAccessor.HttpContext?.User
            .FindAll("permission")
            .Select(c => c.Value)
            .ToArray() ?? Array.Empty<string>();
}
