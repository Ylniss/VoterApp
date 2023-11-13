using System.Net;
using System.Net.Http.Json;
using Shouldly;
using VoterApp.Api.ErrorResponses;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Features.Candidates.Dtos;

namespace VoterApp.IntegrationTests.Api;

public class CandidateApiTests :
    IClassFixture<ApiWebApplicationFactory>
{
    private const string ApiCandidates = "api/candidates";
    private readonly HttpClient _client;

    public CandidateApiTests(
        ApiWebApplicationFactory factory) =>
        _client = factory.CreateClient();

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