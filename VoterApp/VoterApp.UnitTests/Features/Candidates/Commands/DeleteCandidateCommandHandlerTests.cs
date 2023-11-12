using Shouldly;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Features.Candidates.Commands.DeleteCandidate;

namespace VoterApp.UnitTests.Features.Candidates.Commands;

public class DeleteCandidateCommandHandlerTests : IClassFixture<FeatureFixture>
{
    private readonly FeatureFixture _fixture;

    public DeleteCandidateCommandHandlerTests(FeatureFixture fixture) => _fixture = fixture;

    [Fact]
    public void Handle_RepositoryReturnsNull_ShouldThrowNotFoundException()
    {
        // Arrange
        var handler = new DeleteCandidateCommandHandler(_fixture.MockCandidateRepo.Object);

        // Act Assert
        Should.Throw<NotFoundException>(async () =>
            await handler.Handle(new DeleteCandidateCommand(99999), CancellationToken.None));
    }
}