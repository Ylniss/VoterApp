using System.Net;
using System.Net.Http.Json;
using Shouldly;
using VoterApp.Application.Features.Candidates.Dtos;

namespace VoterApp.IntegrationTests.Api;

public class CandidateApiTests :
    IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CandidateApiTests(
        ApiWebApplicationFactory factory) =>
        _client = factory.CreateClient();

    [Fact]
    public async Task Get_Id1ThatExists_ShouldReturnStatusCode200WithObjectThatHasId1()
    {
        // Arrange
        var request = "api/candidates/1";

        // Act
        var response = await _client.GetAsync(request);
        var candidateDto = await response.Content.ReadAsAsync<CandidateDto>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        candidateDto.Id.ShouldBe(1);
    }

    [Fact]
    public async Task Get_IdThatDoesntExist_ShouldReturnStatusCode404()
    {
        // Arrange
        var request = $"api/candidates/{int.MaxValue}";

        // Act
        var response = await _client.GetAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_ValidObjectInBody_ShouldReturnStatusCode201()
    {
        // Arrange
        var request = "api/candidates";
        var body = new
        {
            Name = "John",
            ElectionId = 1
        };

        // Act
        var response = await _client.PostAsync(request, JsonContent.Create(body));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
}