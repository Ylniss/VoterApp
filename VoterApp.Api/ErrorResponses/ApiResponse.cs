namespace VoterApp.Api.ErrorResponses;

public record ApiResponse
{
    public ApiResponse(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GenerateDefaultMessage(statusCode);
    }

    public int StatusCode { get; init; }
    public string? Message { get; init; }

    private string? GenerateDefaultMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad request",
            401 => "You are not authorized",
            404 => "Resource not found",
            500 => "Server error",
            _ => null
        };
    }
}