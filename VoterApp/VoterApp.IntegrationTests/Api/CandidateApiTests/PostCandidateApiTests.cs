using System.Net;
using System.Net.Http.Json;
using Shouldly;
using VoterApp.Api.ErrorResponses;
using VoterApp.Application.Common.Responses;

namespace VoterApp.IntegrationTests.Api.CandidateApiTests;

[Collection("Database collection")]
public class PostCandidateApiTests : IAsyncLifetime
{
    private const string ApiCandidates = "api/candidates";
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public PostCandidateApiTests(
        ApiWebApplicationFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        _client = factory.Client;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();


    [Fact]
    public async Task Post_ValidObjectInBody_ShouldReturnStatusCode201()
    {
        // Arrange
        var request = ApiCandidates;
        var body = new
        {
            Name = "John",
            ElectionId = 1
        };

        // Act
        var response = await _client.PostAsync(request, JsonContent.Create(body));
        var insertedId = (await response.Content.ReadAsAsync<CommandResponse>()).Id;

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        insertedId.ShouldBe(5);
    }

    [Fact]
    public async Task Post_TooLongName_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiCandidates;
        var body = new
        {
            Name = "Very loooooooooooooooooooooooooooooooooooooooooooooooooooooooooong name",
            ElectionId = 1
        };

        // Act
        var response = await _client.PostAsync(request, JsonContent.Create(body));
        var errorResponse = await response.Content.ReadAsAsync<ApiValidationErrorResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorResponse.Errors.Count().ShouldBe(1);
    }

    [Fact]
    public async Task Post_TooShortName_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiCandidates;
        var body = new
        {
            Name = "a",
            ElectionId = 1
        };

        // Act
        var response = await _client.PostAsync(request, JsonContent.Create(body));
        var errorResponse = await response.Content.ReadAsAsync<ApiValidationErrorResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorResponse.Errors.Count().ShouldBe(1);
    }

    [Fact]
    public async Task Post_NameNotUniqueInElectionId1_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiCandidates;
        var body = new
        {
            Name = "Bogdan",
            ElectionId = 1
        };

        // Act
        var response = await _client.PostAsync(request, JsonContent.Create(body));
        var errorResponse = await response.Content.ReadAsAsync<ApiValidationErrorResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorResponse.Errors.Count().ShouldBe(1);
    }

    [Fact]
    public async Task Post_ElectionDoesntExistForCandidate_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiCandidates;
        var body = new
        {
            Name = "Bogdan",
            ElectionId = int.MaxValue
        };

        // Act
        var response = await _client.PostAsync(request, JsonContent.Create(body));
        var errorResponse = await response.Content.ReadAsAsync<ApiValidationErrorResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorResponse.Errors.Count().ShouldBe(1);
    }
}