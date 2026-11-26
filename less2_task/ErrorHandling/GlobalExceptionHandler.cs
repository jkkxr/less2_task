using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace less2_task.ErrorHandling;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService
    ) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request.");

        var time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var method = httpContext.Request.Method;
        var path = httpContext.Request.Path;
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";

        if (exception is RegistrationDisabledException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return await problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Title = "Registration is currently disabled.",
                        Detail = $"[{time}] {method} {path} from {ip} - Exception: {exception.Message}",
                        Type = "https://example.com/probs/registration-disabled"
                    }
                });
        }

        var detailedLogMessage = $"[{time}] {method} {path} from {ip} - Exception: {exception.Message}";

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(
     new ProblemDetailsContext
     {
         HttpContext = httpContext,
         ProblemDetails = new ProblemDetails
         {
             Status = StatusCodes.Status500InternalServerError,
             Title = "An unexpected error occurred.",
             Detail = detailedLogMessage,
             Type = "https://example.com/probs/unexpected-error"
         }
     });
    }
}
