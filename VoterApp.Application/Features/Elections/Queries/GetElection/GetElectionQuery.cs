using AutoMapper;
using MediatR;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Elections.Dtos;

namespace VoterApp.Application.Features.Elections.Queries.GetElection;

public record GetElectionQuery(int Id) : IRequest<ElectionDto>;

public class GetCandidateQueryHandler : IRequestHandler<GetElectionQuery, ElectionDto>
{
    private readonly IElectionRepository _electionRepository;
    private readonly IMapper _mapper;

    public GetCandidateQueryHandler(IMapper mapper, IElectionRepository electionRepository)
    {
        _mapper = mapper;
        _electionRepository = electionRepository;
    }

    public async Task<ElectionDto> Handle(GetElectionQuery request, CancellationToken cancellationToken)
    {
        var election = await _electionRepository.Get(request.Id);

        if (election is null) throw new NotFoundException(request.Id);

        return _mapper.Map<ElectionDto>(election);
    }
}