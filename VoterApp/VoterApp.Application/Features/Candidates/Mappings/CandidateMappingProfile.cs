using AutoMapper;
using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;
using VoterApp.Application.Features.Candidates.Commands.UpdateCandidate;
using VoterApp.Application.Features.Candidates.Dtos;
using VoterApp.Domain.Entities;

namespace VoterApp.Application.Features.Candidates.Mappings;

public class CandidateMappingProfile : Profile
{
    public CandidateMappingProfile()
    {
        CreateMap<Candidate, CandidateDto>()
            .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Voters.Count))
            .ConstructUsing(candidate => new CandidateDto(
                candidate.Id,
                candidate.Name,
                candidate.Election.Id,
                candidate.Voters.Count
            ));

        CreateMap<CreateCandidateDto, CreateCandidateCommand>();
        CreateMap<UpdateCandidateDto, UpdateCandidateCommand>()
            .ForMember(x => x.Id, opt => opt.Ignore());
    }
}