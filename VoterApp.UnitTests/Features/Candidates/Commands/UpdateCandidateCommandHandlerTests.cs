using Shouldly;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Features.Candidates.Commands.UpdateCandidate;

namespace VoterApp.UnitTests.Features.Candidates.Commands;

public class UpdateCandidateCommandHandlerTests : IClassFixture<FeatureFixture>
{
    private readonly FeatureFixture _fixture;

    public UpdateCandidateCommandHandlerTests(FeatureFixture fixture) => _fixture = fixture;

    [Fact]
    public void Handle_RepositoryReturnsNull_ShouldThrowNotFoundException()
    {
        // Arrange
        var handler = new UpdateCandidateCommandHandler(_fixture.MockCandidateRepo.Object);

        // Act Assert
        Should.Throw<NotFoundException>(async () =>
            await handler.Handle(new UpdateCandidateCommand { Id = 999999, Name = "Bob" }, CancellationToken.None)
        );
    }
}