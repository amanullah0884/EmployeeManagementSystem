using System.Security.Claims;
using EmployeeManagement.Application.Abstractions;

namespace EmployeeManagement.WebAPI.Middleware;

public sealed class RequestAuditMiddleware
{
    private readonly RequestDelegate _next;

    public RequestAuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Request.Path.StartsWithSegments("/swagger"))
            return;

        if (!context.Request.Path.StartsWithSegments("/api"))
            return;

        if (context.Request.Path.StartsWithSegments("/api/auth/login"))
            return;

        if (context.Request.Path.StartsWithSegments("/api/auth/logout"))
            return;

        using var scope = context.RequestServices.CreateScope();
        var writer = scope.ServiceProvider.GetRequiredService<IAuditTrailWriter>();

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = context.User.FindFirstValue(ClaimTypes.Name)
            ?? context.User.FindFirstValue(ClaimTypes.Email);

        await writer.WriteAsync(
            "Http",
            $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}",
            "HttpRequest",
            null,
            $"StatusCode={context.Response.StatusCode}",
            int.TryParse(userId, out var id) ? id : null,
            username,
            context.TraceIdentifier,
            context.RequestAborted);
    }
}
