using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

public class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var extensions = new Dictionary<string, object?>
        {
            ["traceId"] = Activity.Current?.Id,
        };

        //If your logger logs DiagnosticsTelemetry, you should remove the string below to avoid the exception being logged twice.
        logger.LogError(exception, "");


        var result = exception switch
        {
            BadHttpRequestException ex => TypedResults.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad request",
                detail: ex.Message,
                extensions: extensions,
                type: "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
            _ => TypedResults.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unhandled exception has occurred while executing the request",
                detail: exception.Message,
                extensions: extensions,
                type: "https://tools.ietf.org/html/rfc7231#section-6.6.1")
        };

        await result.ExecuteAsync(httpContext).ConfigureAwait(false);
        return true;
    }
}
