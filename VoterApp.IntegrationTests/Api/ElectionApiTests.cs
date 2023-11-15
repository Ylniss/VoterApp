using System.Net;
using System.Net.Http.Json;
using Shouldly;
using VoterApp.Api.ErrorResponses;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Features.Elections.Dtos;

namespace VoterApp.IntegrationTests.Api;

[Collection(nameof(DatabaseCollection))]
public class ElectionApiTests :
    IClassFixture<ApiWebApplicationFactory>
{
    private const string ApiElections = "api/elections";
    private readonly HttpClient _client;

    public ElectionApiTests(
        ApiWebApplicationFactory factory) =>
        _client = factory.CreateClient();

    [Fact]
    public async Task Get_Id1ThatExists_ShouldReturnStatusCode200WithObjectThatHasId1()
    {
        // Arrange
        var request = $"{ApiElections}/1";

        // Act
        var response = await _client.GetAsync(request);
        var electionDto = await response.Content.ReadAsAsync<ElectionDto>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        electionDto.Id.ShouldBe(1);
    }

    [Fact]
    public async Task Get_IdThatDoesntExist_ShouldReturnStatusCode404()
    {
        // Arrange
        var request = $"{ApiElections}/{int.MaxValue}";

        // Act
        var response = await _client.GetAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetByRoomCode_RoomCodeThatExists_ShouldReturnStatusCode200WithObjectThatHasId1()
    {
        // Arrange
        var request = $"{ApiElections}/roomcode/c7f8b63d-4ca7-41f8-bd28-54ff5d41dc13";

        // Act
        var response = await _client.GetAsync(request);
        var electionPublicDto = await response.Content.ReadAsAsync<ElectionPublicDto>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        electionPublicDto.Topic.ShouldBe("Choose your man");
    }

    [Fact]
    public async Task GetByRoomCode_RoomCodeThatDoesntExist_ShouldReturnStatusCode404()
    {
        // Arrange
        var request = $"{ApiElections}/roomcode/3fa85f64-5717-4562-b3fc-2c963f66afa6";

        // Act
        var response = await _client.GetAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_ValidObjectInBody_ShouldReturnStatusCode201()
    {
        // Arrange
        var request = ApiElections;
        var body = new
        {
            Topic = "Election for pope"
        };

        // Act
        var response = await _client.PostAsync(request, JsonContent.Create(body));
        var insertedId = (await response.Content.ReadAsAsync<CommandResponse>()).Id;

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        insertedId.ShouldBe(3);
    }

    [Fact]
    public async Task Post_TooLongName_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiElections;
        var body = new
        {
            Topic =
                "Super Long Election Name Super Long Election Name Super Long Election Name Super Long Election Name" +
                "Super Long Election Name Super Long Election Name Super Long Election Name Super Long Election Name" +
                "Super Long Election Name Super Long Election Name Super Long Election Name Super Long Election Name" +
                "Super Long Election Name Super Long Election Name Super Long Election Name Super Long Election Name" +
                "Super Long Election Name Super Long Election Name Super Long Election Name Super Long Election Name" +
                "Super Long Election Name Super Long Election Name Super Long Election Name Super Long Election Name"
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
        var request = ApiElections;
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
}