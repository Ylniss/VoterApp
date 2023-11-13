using AutoMapper;
using Moq;
using VoterApp.Application.Contracts;
using VoterApp.UnitTests.Mappings;

namespace VoterApp.UnitTests.Features;

public class FeatureFixture
{
    public FeatureFixture()
    {
        MockCandidateRepo = new Mock<ICandidateRepository>();

        MockCandidateRepo.Setup(r => r.Get(It.IsAny<int>(), null)).ReturnsAsync(() => null);

        Mapper = MapperProvider.CreateMapper();
    }

    public Mock<ICandidateRepository> MockCandidateRepo { get; }
    public IMapper Mapper { get; private set; }
}