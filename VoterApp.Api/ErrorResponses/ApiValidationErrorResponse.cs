namespace VoterApp.Api.ErrorResponses;

public record ApiValidationErrorResponse() : ApiResponse(400)
{
    public IEnumerable<string>? Errors { get; init; }
}