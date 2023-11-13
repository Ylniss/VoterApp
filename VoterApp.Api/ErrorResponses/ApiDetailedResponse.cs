namespace VoterApp.Api.ErrorResponses;

public record ApiDetailedResponse : ApiResponse
{
    public ApiDetailedResponse(int statusCode, string? message = null, string? details = null) : base(statusCode,
        message)
    {
        Details = details;
    }

    public string? Details { get; init; }
}