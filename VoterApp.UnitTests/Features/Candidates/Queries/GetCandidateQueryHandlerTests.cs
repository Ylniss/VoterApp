using Shouldly;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Features.Candidates.Queries.GetCandidate;

namespace VoterApp.UnitTests.Features.Candidates.Queries;

public class GetCandidateQueryHandlerTests : IClassFixture<FeatureFixture>
{
    private readonly FeatureFixture _fixture;

    public GetCandidateQueryHandlerTests(FeatureFixture fixture) => _fixture = fixture;

    [Fact]
    public void Handle_RepositoryReturnsNull_ShouldThrowNotFoundException()
    {
        // Arrange
        var handler = new GetCandidateQueryHandler(_fixture.Mapper, _fixture.MockCandidateRepo.Object);

        // Act Assert
        Should.Throw<NotFoundException>(async () =>
            await handler.Handle(new GetCandidateQuery(Id: 2000), CancellationToken.None));
    }
}