using EmployeeManagement.Application.Abstractions;
using EmployeeManagement.Shared.Api;
using System.Net;
using System.Text.Json;

namespace EmployeeManagement.WebAPI.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception. Trace={TraceId}", context.TraceIdentifier);

            using (var scope = context.RequestServices.CreateScope())
            {
                var writer = scope.ServiceProvider.GetService<IAuditTrailWriter>();
                if (writer is not null)
                {
                    try
                    {
                        await writer.WriteAsync(
                            "Error",
                            "UnhandledException",
                            ex.GetType().Name,
                            null,
                            ex.Message,
                            null,
                            null,
                            context.TraceIdentifier,
                            context.RequestAborted);
                    }
                    catch (Exception auditEx)
                    {
                        _logger.LogWarning(auditEx, "Failed to persist error audit record.");
                    }
                }
            }

            if (context.Response.HasStarted)
                throw;

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var body = StandardResponseFactory.Failure(
                "An unexpected error occurred.",
                "500",
                new[]
                {
                    new ErrorDetails
                    {
                        Message = ex.Message,
                        Code = ex.GetType().Name
                    }
                });

            var json = JsonSerializer.Serialize(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await context.Response.WriteAsync(json, context.RequestAborted);
        }
    }
}
