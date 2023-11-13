using Moq;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;
using VoterApp.Domain.Entities;

namespace VoterApp.UnitTests.FluentValidators;

public class ValidatorFixture
{
    public ValidatorFixture()
    {
        ValidCreateCandidateCommand = new CreateCandidateCommand("Valid Name", 1);

        MockCandidateRepo = new Mock<ICandidateRepository>();
        MockElectionRepo = new Mock<IElectionRepository>();

        MockCandidateRepo.Setup(r => r.GetAll(null))
            .ReturnsAsync(() => new List<Candidate> { new("Same Name", new Election("topic")) });

        MockElectionRepo.Setup(r => r.Get(It.IsAny<int>(), null)).ReturnsAsync(() => new Election("Topic name"));
    }

    public Mock<ICandidateRepository> MockCandidateRepo { get; }
    public Mock<IElectionRepository> MockElectionRepo { get; }
    public CreateCandidateCommand ValidCreateCandidateCommand { get; private set; }
}