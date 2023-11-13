using AutoMapper;
using VoterApp.Application.Features.Voters.Commands.CreateVoter;
using VoterApp.Application.Features.Voters.Commands.UpdateVoterName;
using VoterApp.Application.Features.Voters.Commands.Vote;
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

        CreateMap<Voter, UpdateVoterCommand>()
            .ForMember(dest => dest.VotedCandidateId, opt => opt.MapFrom(src => src.VotedCandidate.Id))
            .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src => src.Election.Id));

        CreateMap<VoteDto, VoteCommand>();

        CreateMap<CreateVoterDto, CreateVoterCommand>();
        CreateMap<UpdateVoterDto, UpdateVoterCommand>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.ElectionId, opt => opt.Ignore());
    }
}