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
        CreateMap<CandidateDto, Candidate>().ReverseMap();

        CreateMap<CreateCandidateDto, CreateCandidateCommand>().ReverseMap();
        CreateMap<UpdateCandidateDto, UpdateCandidateCommand>().ReverseMap();
    }
}