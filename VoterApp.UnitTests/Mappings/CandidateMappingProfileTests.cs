using Shouldly;
using VoterApp.Application.Features.Candidates.Dtos;

namespace VoterApp.UnitTests.Mappings;

public class CandidateMappingProfileTests : IClassFixture<AutoMapperFixture>
{
    private readonly AutoMapperFixture _fixture;

    public CandidateMappingProfileTests(AutoMapperFixture fixture) => _fixture = fixture;

    [Fact]
    public void Mapper_ConfigurationShouldBeValid()
    {
        // Arrange
        var config = _fixture.Configuration;

        // Assert
        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_ValidCandidateToDto_ShouldReturnValidDto()
    {
        // Arrange
        var candidate = _fixture.ValidCandidate;
        var expectedDto = _fixture.ValidCandidateDto;

        // Act
        var result = _fixture.Mapper.Map<CandidateDto>(candidate);

        // Assert
        result.ShouldBeEquivalentTo(expectedDto);
    }
}