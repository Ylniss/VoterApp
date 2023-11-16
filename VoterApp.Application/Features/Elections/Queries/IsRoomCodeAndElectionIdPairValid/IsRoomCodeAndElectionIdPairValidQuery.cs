using AutoMapper;
using MediatR;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Elections.Queries.IsRoomCodeAndElectionIdPairValid;

public record IsRoomCodeAndElectionIdPairValidQuery(Guid RoomCode, int ElectionId) : IRequest<bool>;

public class IsRoomCodeAndElectionIdPairValidQueryHandler : IRequestHandler<IsRoomCodeAndElectionIdPairValidQuery, bool>
{
    private readonly IElectionRepository _electionRepository;
    private readonly IMapper _mapper;

    public IsRoomCodeAndElectionIdPairValidQueryHandler(IMapper mapper, IElectionRepository electionRepository)
    {
        _mapper = mapper;
        _electionRepository = electionRepository;
    }

    public async Task<bool> Handle(IsRoomCodeAndElectionIdPairValidQuery request, CancellationToken cancellationToken)
    {
        var election = await _electionRepository.Get(request.ElectionId);

        if (election is null) throw new NotFoundException(request.ElectionId);

        return election.RoomCode == request.RoomCode && election.Id == request.ElectionId;
    }
}