using System.Text.Json;
using FluentValidation;
using VoterApp.Api.ErrorResponses;
using VoterApp.Application.Common.Exceptions;

namespace VoterApp.Api.ExceptionHandlers;

public class ExceptionHandler : IExceptionHandler
{
    private readonly IHostEnvironment _env;
    private readonly IDictionary<Type, Func<Exception, HttpContext, JsonSerializerOptions, string>> _exceptionHandlers;

    public ExceptionHandler(IHostEnvironment env)
    {
        _env = env;

        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Func<Exception, HttpContext, JsonSerializerOptions, string>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException }
        };
    }

    public string HandleException(Exception exception, HttpContext context)
    {
        context.Response.ContentType = "application/json";

        // set camel case naming for consistency
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var type = exception.GetType();
        if (_exceptionHandlers.ContainsKey(type)) return _exceptionHandlers[type].Invoke(exception, context, options);

        return HandleUnknownException(exception, context, options);
    }

    private string HandleValidationException(Exception exception, HttpContext context, JsonSerializerOptions options)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        var errors = ((ValidationException)exception).Errors.Select(x => x.ErrorMessage);
        var response = new ApiValidationErrorResponse { Errors = errors };

        return JsonSerializer.Serialize(response, options);
    }

    private string HandleNotFoundException(Exception exception, HttpContext context, JsonSerializerOptions options)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        var response = _env.IsDevelopment()
            ? new ApiResponse(StatusCodes.Status404NotFound, exception.Message)
            : new ApiResponse(StatusCodes.Status404NotFound);

        return JsonSerializer.Serialize(response, options);
    }

    private string HandleUnauthorizedAccessException(Exception exception, HttpContext context,
        JsonSerializerOptions options)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        var response = _env.IsDevelopment()
            ? new ApiResponse(StatusCodes.Status401Unauthorized, exception.Message)
            : new ApiResponse(StatusCodes.Status401Unauthorized);

        return JsonSerializer.Serialize(response, options);
    }

    private string HandleUnknownException(Exception exception, HttpContext context, JsonSerializerOptions options)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = _env.IsDevelopment()
            ? new ApiDetailedResponse(StatusCodes.Status500InternalServerError, exception.Message, exception.StackTrace)
            : new ApiResponse(StatusCodes.Status500InternalServerError);

        return JsonSerializer.Serialize(response, options);
    }
}