using FluentResults;
using Microsoft.AspNetCore.Diagnostics;

namespace API.SharedAPI;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problem = new Error("Internal error");
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}