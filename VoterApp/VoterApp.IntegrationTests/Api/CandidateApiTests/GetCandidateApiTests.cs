using System.Net;
using Shouldly;
using VoterApp.Application.Features.Candidates.Dtos;

namespace VoterApp.IntegrationTests.Api.CandidateApiTests;

[Collection("Database collection")]
public class GetCandidateApiTests : IAsyncLifetime
{
    private const string ApiCandidates = "api/candidates";
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public GetCandidateApiTests(
        ApiWebApplicationFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        _client = factory.Client;
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();

    [Fact]
    public async Task Get_Id1ThatExists_ShouldReturnStatusCode200WithObjectThatHasId1()
    {
        // Arrange
        var request = $"{ApiCandidates}/1";

        // Act
        var response = await _client.GetAsync(request);
        var candidateDto = await response.Content.ReadAsAsync<CandidateDto>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        candidateDto.Id.ShouldBe(1);
    }

    [Fact]
    public async Task Get_Id1ThatExists_ShouldReturnStatusCode200WithObjectThatHasId1_50Parallel()
    {
        // Arrange
        var request = $"{ApiCandidates}/1";

        // Act
        var tasks = new List<Task<HttpResponseMessage>>();
        for (var i = 0; i < 50; ++i) tasks.Add(Task.Run(() => _client.GetAsync(request)));

        var responses = await Task.WhenAll(tasks);

        foreach (var response in responses)
        {
            var candidateDto = await response.Content.ReadAsAsync<CandidateDto>();

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            candidateDto.Id.ShouldBe(1);
        }
    }

    [Fact]
    public async Task GetAll_ShouldReturn4Objects()
    {
        // Arrange
        var request = ApiCandidates;

        // Act
        var response = await _client.GetAsync(request);
        var candidates = await response.Content.ReadAsAsync<IEnumerable<CandidateDto>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        candidates.Count().ShouldBe(4);
    }

    [Fact]
    public async Task Get_IdThatDoesntExist_ShouldReturnStatusCode404()
    {
        // Arrange
        var request = $"{ApiCandidates}/{int.MaxValue}";

        // Act
        var response = await _client.GetAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}