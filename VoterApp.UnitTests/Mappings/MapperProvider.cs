using AutoMapper;
using VoterApp.Application.Features.Candidates.Mappings;

namespace VoterApp.UnitTests.Mappings;

public class MapperProvider
{
    public static MapperConfiguration CreateConfig()
    {
        return new MapperConfiguration(cfg => cfg.AddProfile<CandidateMappingProfile>());
    }

    public static IMapper CreateMapper()
    {
        var config = CreateConfig();
        return config.CreateMapper();
    }
}