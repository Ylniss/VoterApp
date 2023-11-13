namespace VoterApp.Api.ExceptionHandlers;

public interface IExceptionHandler
{
    string HandleException(Exception exception, HttpContext context);
}