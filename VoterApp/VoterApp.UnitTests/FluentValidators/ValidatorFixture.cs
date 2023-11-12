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

        MockCandidateRepo.Setup(r => r.GetAll())
            .ReturnsAsync(() => new List<Candidate> { new("Same Name", new Election("topic")) });
    }

    public Mock<ICandidateRepository> MockCandidateRepo { get; }
    public CreateCandidateCommand ValidCreateCandidateCommand { get; private set; }
}