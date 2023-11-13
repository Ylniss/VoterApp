using System.Net;
using Shouldly;

namespace VoterApp.IntegrationTests.Api.CandidateApiTests;

[Collection("Database collection")]
public class DeleteCandidateApiTests : IAsyncLifetime
{
    private const string ApiCandidates = "api/candidates";
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public DeleteCandidateApiTests(
        ApiWebApplicationFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        _client = factory.Client;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();

    [Fact]
    public async Task Delete_Id1ThatExists_ShouldReturnStatusCode200()
    {
        // Arrange
        var request = $"{ApiCandidates}/1";

        // Act
        var response = await _client.DeleteAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_IdThatDoesntExist_ShouldReturnStatusCode404()
    {
        // Arrange
        var request = $"{ApiCandidates}/{int.MaxValue}";

        // Act
        var response = await _client.DeleteAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}