using AutoMapper;
using VoterApp.Application.Features.Voters.Dtos;
using VoterApp.Domain.Entities;

namespace VoterApp.Application.Features.Voters.Mappings;

public class VoterMappingProfile : Profile
{
    public VoterMappingProfile()
    {
        CreateMap<Voter, VoterDto>()
            .ConstructUsing(voter => new VoterDto(
                voter.Id,
                voter.Name,
                voter.Election.Id,
                voter.KeyPhrase,
                voter.HasVoted
            ));

        // CreateMap<CreateVoterDto, CreateVoterCommand>();
        // CreateMap<UpdateVoterDto, UpdateVoterCommand>()
        //     .ForMember(x => x.Id, opt => opt.Ignore());
    }
}