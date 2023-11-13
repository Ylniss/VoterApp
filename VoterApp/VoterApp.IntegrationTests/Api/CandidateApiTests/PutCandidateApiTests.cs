using System.Net;
using System.Net.Http.Json;
using Shouldly;

namespace VoterApp.IntegrationTests.Api.CandidateApiTests;

[Collection("Database collection")]
public class PutCandidateApiTests : IAsyncLifetime
{
    private const string ApiCandidates = "api/candidates";
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public PutCandidateApiTests(
        ApiWebApplicationFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        _client = factory.Client;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();

    [Fact]
    public async Task Put_UpdateObjectId1_ShouldReturnStatusCode200()
    {
        // Arrange
        var request = $"{ApiCandidates}/1";
        var body = new
        {
            Name = "NewBogdan",
            ElectionId = 1
        };

        // Act
        var response = await _client.PutAsync(request, JsonContent.Create(body));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_IdThatDoesntExist_ShouldReturnStatusCode404()
    {
        // Arrange
        var request = $"{ApiCandidates}/{int.MaxValue}";
        var body = new
        {
            Name = "NewBogdan",
            ElectionId = 1
        };

        // Act
        var response = await _client.PutAsync(request, JsonContent.Create(body));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}