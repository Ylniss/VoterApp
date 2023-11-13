using System.Net;
using System.Net.Http.Json;
using Shouldly;
using VoterApp.Api.ErrorResponses;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Features.Voters.Dtos;

namespace VoterApp.IntegrationTests.Api;

[Collection(nameof(DatabaseCollection))]
public class VoterApiTests :
    IClassFixture<ApiWebApplicationFactory>
{
    private const string ApiVoters = "api/voters";
    private readonly HttpClient _client;

    public VoterApiTests(
        ApiWebApplicationFactory factory) =>
        _client = factory.CreateClient();

    [Fact]
    public async Task Get_Id1ThatExists_ShouldReturnStatusCode200WithObjectThatHasNameChillman()
    {
        // Arrange
        var request = $"{ApiVoters}/1";

        // Act
        var response = await _client.GetAsync(request);
        var voterDto = await response.Content.ReadAsAsync<VoterDto>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        voterDto.Name.ShouldBe("Chillman");
    }

    [Fact]
    public async Task Get_IdThatDoesntExist_ShouldReturnStatusCode404()
    {
        // Arrange
        var request = $"{ApiVoters}/{int.MaxValue}";

        // Act
        var response = await _client.GetAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_ShouldReturnMoreOrEqualTo4Objects()
    {
        // Arrange
        var request = ApiVoters;

        // Act
        var response = await _client.GetAsync(request);
        var voters = await response.Content.ReadAsAsync<IEnumerable<VoterDto>>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        voters.Count().ShouldBeGreaterThanOrEqualTo(4);
    }

    [Fact]
    public async Task Post_ValidObjectInBody_ShouldReturnStatusCode201()
    {
        // Arrange
        var request = ApiVoters;
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
        insertedId.ShouldBeGreaterThanOrEqualTo(7);
    }

    [Fact]
    public async Task Post_TooLongName_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiVoters;
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
        var request = ApiVoters;
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
        var request = ApiVoters;
        var body = new
        {
            Name = "Chillman",
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
    public async Task Post_ElectionDoesntExistForVoter_ShouldReturnStatusCode400With1ValidationError()
    {
        // Arrange
        var request = ApiVoters;
        var body = new
        {
            Name = "Ziommm",
            ElectionId = int.MaxValue
        };

        // Act
        var response = await _client.PostAsync(request, JsonContent.Create(body));
        var errorResponse = await response.Content.ReadAsAsync<ApiValidationErrorResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errorResponse.Errors.Count().ShouldBe(1);
    }

    [Fact]
    public async Task Put_UpdateObjectId2_ShouldReturnStatusCode200()
    {
        // Arrange
        var request = $"{ApiVoters}/2";
        var body = new
        {
            Name = "SuperLilchan",
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
        var request = $"{ApiVoters}/{int.MaxValue}";
        var body = new
        {
            Name = "SuperChillman2000",
            ElectionId = 1
        };

        // Act
        var response = await _client.PutAsync(request, JsonContent.Create(body));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_IdThatDoesntExist_ShouldReturnStatusCode404()
    {
        // Arrange
        var request = $"{ApiVoters}/{int.MaxValue}";

        // Act
        var response = await _client.DeleteAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}