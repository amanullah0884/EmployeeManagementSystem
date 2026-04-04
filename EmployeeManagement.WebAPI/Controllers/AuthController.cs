using EmployeeManagement.Application.Abstractions;
using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Api;
using EmployeeManagement.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUser;
    private readonly IAuthenticationActivityLogger _authenticationActivityLogger;

    public AuthController(
        IAuthService authService,
        ICurrentUserService currentUser,
        IAuthenticationActivityLogger authenticationActivityLogger)
    {
        _authService = authService;
        _currentUser = currentUser;
        _authenticationActivityLogger = authenticationActivityLogger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var result = await _authService.LoginAsync(request, correlationId, cancellationToken);
        return this.ToStandardLogin(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? 0;
        await _authenticationActivityLogger.LogLogoutAsync(userId, _currentUser.Username, HttpContext.TraceIdentifier, cancellationToken);

        return Ok(StandardResponseFactory.Success(
            (object?)null,
            "Logout successful. The client must discard the stored JWT; authorization uses database-driven permissions embedded in the token at login.",
            "200"));
    }

    [HttpPost("register")]
    [Authorize(Policy = AppPermissions.UsersManage)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);
        if (!result.Success)
            return BadRequest(StandardResponseFactory.Failure(result.Error!, "400"));

        return Ok(StandardResponseFactory.Success(new { id = result.Data }, "User registered successfully", "200"));
    }
}
