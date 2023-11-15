using AutoMapper;
using MediatR;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Elections.Dtos;

namespace VoterApp.Application.Features.Elections.Queries.GetElection;

public record GetElectionByRoomCodeQuery(Guid RoomCode) : IRequest<ElectionPublicDto>;

public class GetElectionByRoomCodeHandler : IRequestHandler<GetElectionByRoomCodeQuery, ElectionPublicDto>
{
    private readonly IElectionRepository _electionRepository;
    private readonly IMapper _mapper;

    public GetElectionByRoomCodeHandler(IMapper mapper, IElectionRepository electionRepository)
    {
        _mapper = mapper;
        _electionRepository = electionRepository;
    }

    public async Task<ElectionPublicDto> Handle(GetElectionByRoomCodeQuery request, CancellationToken cancellationToken)
    {
        var election = await _electionRepository.GetByRoomCode(request.RoomCode);

        if (election is null) throw new NotFoundException(request.RoomCode.ToString());

        return _mapper.Map<ElectionPublicDto>(election);
    }
}