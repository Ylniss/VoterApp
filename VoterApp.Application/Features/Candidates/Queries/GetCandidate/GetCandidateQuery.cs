using AutoMapper;
using MediatR;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Candidates.Dtos;

namespace VoterApp.Application.Features.Candidates.Queries.GetCandidate;

public record GetCandidateQuery(int Id) : IRequest<CandidateDto>;

public class GetCandidateQueryHandler : IRequestHandler<GetCandidateQuery, CandidateDto>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;

    public GetCandidateQueryHandler(IMapper mapper, ICandidateRepository candidateRepository)
    {
        _mapper = mapper;
        _candidateRepository = candidateRepository;
    }

    public async Task<CandidateDto> Handle(GetCandidateQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.Get(request.Id);

        if (candidate is null) throw new NotFoundException(request.Id);

        return _mapper.Map<CandidateDto>(candidate);
    }
}