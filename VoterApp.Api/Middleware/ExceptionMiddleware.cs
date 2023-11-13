using VoterApp.Api.ExceptionHandlers;

namespace VoterApp.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly IExceptionHandler _exceptionsHandler;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IExceptionHandler exceptionHandler)
    {
        _next = next;
        _logger = logger;
        _exceptionsHandler = exceptionHandler;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            await WriteErrorResponse(ex, context);
        }
    }

    private async Task WriteErrorResponse(Exception exception, HttpContext context)
    {
        var json = _exceptionsHandler.HandleException(exception, context);

        await context.Response.WriteAsync(json);
    }
}