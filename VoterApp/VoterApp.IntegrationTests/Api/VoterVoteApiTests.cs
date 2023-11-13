using System.Net;
using System.Net.Http.Json;
using Shouldly;
using VoterApp.Api.ErrorResponses;

namespace VoterApp.IntegrationTests.Api;

[Collection(nameof(DatabaseCollection))]
public class VoterVoteApiTests :
    IClassFixture<ApiWebApplicationFactory>
{
    private const string ApiVotersVote = "api/voters/vote";
    private readonly HttpClient _client;

    public VoterVoteApiTests(
        ApiWebApplicationFactory factory) =>
        _client = factory.CreateClient();

    [Fact]
    public async Task Put_VoteForValidCandidate_ShouldReturnStatusCode200()
    {
        // Arrange
        var request = ApiVotersVote;
        var body = new
        {
            VoterId = 4,
            CandidateId = 4,
            KeyPhrase = "997"
        };

        // Act
        var response = await _client.PutAsync(request, JsonContent.Create(body));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_VoterAndCandidateDoesntExist_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiVotersVote;
        var body = new
        {
            VoterId = int.MaxValue,
            CandidateId = int.MaxValue,
            KeyPhrase = "997"
        };

        // Act
        var response = await _client.PutAsync(request, JsonContent.Create(body));
        var errorResponse = await response.Content.ReadAsAsync<ApiValidationErrorResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorResponse.Errors.Count().ShouldBe(1);
    }

    [Fact]
    public async Task Put_IncorrectKeyPhrase_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiVotersVote;
        var body = new
        {
            VoterId = 6,
            CandidateId = 4,
            KeyPhrase = "incorrect"
        };

        // Act
        var response = await _client.PutAsync(request, JsonContent.Create(body));
        var errorResponse = await response.Content.ReadAsAsync<ApiValidationErrorResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorResponse.Errors.Count().ShouldBe(1);
    }

    [Fact]
    public async Task Put_VoterHasAlreadyVoted_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiVotersVote;
        var body = new
        {
            VoterId = 1,
            CandidateId = 3,
            KeyPhrase = "123"
        };

        // Act
        var response = await _client.PutAsync(request, JsonContent.Create(body));
        var errorResponse = await response.Content.ReadAsAsync<ApiValidationErrorResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorResponse.Errors.Count().ShouldBe(1);
    }

    [Fact]
    public async Task Put_VoterIsInDifferentElectionThanCandidate_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiVotersVote;
        var body = new
        {
            VoterId = 5,
            CandidateId = 1,
            KeyPhrase = "sikret"
        };

        // Act
        var response = await _client.PutAsync(request, JsonContent.Create(body));
        var errorResponse = await response.Content.ReadAsAsync<ApiValidationErrorResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorResponse.Errors.Count().ShouldBe(1);
    }
}