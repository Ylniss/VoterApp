using AutoMapper;
using MediatR;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Candidates.Dtos;

namespace VoterApp.Application.Features.Candidates.Queries.GetAllCandidates;

public record GetAllCandidatesQuery : IRequest<IEnumerable<CandidateDto>>;

public class GetAllCandidatesQueryHandler : IRequestHandler<GetAllCandidatesQuery, IEnumerable<CandidateDto>>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;

    public GetAllCandidatesQueryHandler(IMapper mapper, ICandidateRepository candidateRepository)
    {
        _mapper = mapper;
        _candidateRepository = candidateRepository;
    }

    public async Task<IEnumerable<CandidateDto>> Handle(GetAllCandidatesQuery request,
        CancellationToken cancellationToken)
    {
        var candidates = await _candidateRepository.GetAll();

        return _mapper.Map<IEnumerable<CandidateDto>>(candidates);
    }
}