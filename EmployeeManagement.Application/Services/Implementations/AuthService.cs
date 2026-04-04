using EmployeeManagement.Application.Abstractions;
using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IAuthenticationActivityLogger _authenticationActivityLogger;

    public AuthService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IAuthenticationActivityLogger authenticationActivityLogger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _authenticationActivityLogger = authenticationActivityLogger;
    }

    public async Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest request, string? correlationId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            await _authenticationActivityLogger.LogLoginFailureAsync(request.Username, "Invalid username or password.", correlationId, cancellationToken);
            return ApiResult<LoginResponse>.Fail("Invalid username or password.");
        }

        var role = await _unitOfWork.Roles.GetByIdWithPermissionsAsync(user.RoleId, cancellationToken);
        var permissions = role?.Permissions.Select(p => p.Name).ToList() ?? new List<string>();
        var token = _jwtTokenGenerator.GenerateToken(user, permissions);

        await _authenticationActivityLogger.LogLoginSuccessAsync(user.Id, user.Username, correlationId, cancellationToken);

        return ApiResult<LoginResponse>.Ok(new LoginResponse(
            token,
            user.Username,
            role?.Name ?? string.Empty,
            permissions));
    }

    public async Task<ApiResult<int>> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.UsernameExistsAsync(request.Username, cancellationToken))
            return ApiResult<int>.Fail("Username is already taken.");

        var role = await _unitOfWork.Roles.GetByIdWithPermissionsAsync(request.RoleId, cancellationToken);
        if (role is null)
            return ApiResult<int>.Fail("Role not found.");

        var user = new Domain.Models.User
        {
            Username = request.Username.Trim(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            RoleId = request.RoleId
        };

        _unitOfWork.Users.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResult<int>.Ok(user.Id);
    }
}
