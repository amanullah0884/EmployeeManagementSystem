using System.Security.Claims;
using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Domain.Models;

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
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = context.User.FindFirstValue(ClaimTypes.Name)
            ?? context.User.FindFirstValue(ClaimTypes.Email);

        uow.AuditLogs.Add(new AuditLog
        {
            EventCategory = "Http",
            UserId = int.TryParse(userId, out var id) ? id : null,
            Username = username,
            Action = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}",
            EntityName = "HttpRequest",
            EntityId = null,
            Details = $"StatusCode={context.Response.StatusCode}",
            CorrelationId = context.TraceIdentifier,
            CreatedAtUtc = DateTime.UtcNow
        });

        await uow.SaveChangesAsync(context.RequestAborted);
    }
}
