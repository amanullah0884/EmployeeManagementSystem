using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest request, string? correlationId, CancellationToken cancellationToken = default);

    Task<ApiResult<int>> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
}
