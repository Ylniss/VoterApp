using AutoMapper;
using VoterApp.Application.Features.Candidates.Dtos;
using VoterApp.Domain.Entities;

namespace VoterApp.UnitTests.Mappings;

public class AutoMapperFixture
{
    public AutoMapperFixture()
    {
        Configuration = MapperProvider.CreateConfig();
        Mapper = Configuration.CreateMapper();

        ValidCandidate = new Candidate("Jack", new Election("topic") { Id = 1 })
        {
            Id = 1
        };

        ValidCandidateDto = new CandidateDto(1, "Jack", 1, 0);
    }

    public MapperConfiguration Configuration { get; }
    public IMapper Mapper { get; private set; }

    public Candidate ValidCandidate { get; private set; }
    public CandidateDto ValidCandidateDto { get; private set; }
}