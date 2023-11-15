using AutoMapper;
using VoterApp.Application.Features.Elections.Commands.CreateElection;
using VoterApp.Application.Features.Elections.Dtos;
using VoterApp.Domain.Entities;

namespace VoterApp.Application.Features.Elections.Mappings;

public class ElectionMappingProfile : Profile
{
    public ElectionMappingProfile()
    {
        CreateMap<Election, ElectionDto>()
            .ForMember(dest => dest.Candidates, opt => opt.MapFrom(src => src.Candidates))
            .ForMember(dest => dest.Voters, opt => opt.MapFrom(src => src.Voters));

        CreateMap<Election, ElectionPublicDto>()
            .ForMember(dest => dest.Candidates, opt => opt.MapFrom(src => src.Candidates))
            .ForMember(dest => dest.Voters, opt => opt.MapFrom(src => src.Voters));

        CreateMap<CreateElectionDto, CreateElectionCommand>();
    }
}