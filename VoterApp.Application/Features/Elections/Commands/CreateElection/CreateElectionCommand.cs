using MediatR;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Elections.Dtos;

namespace VoterApp.Application.Features.Elections.Commands.CreateElection;

public record CreateElectionCommand(string Topic) : IRequest<CreateElectionCommandResponse>;

public class CreateElectionCommandHandler : IRequestHandler<CreateElectionCommand, CreateElectionCommandResponse>
{
    private readonly IElectionRepository _electionRepository;

    public CreateElectionCommandHandler(IElectionRepository electionRepository) =>
        _electionRepository = electionRepository;

    public async Task<CreateElectionCommandResponse> Handle(CreateElectionCommand request,
        CancellationToken cancellationToken)
    {
        var id = await _electionRepository.Create(request);
        var roomCode = (await _electionRepository.Get(id))?.RoomCode;

        return new CreateElectionCommandResponse(id, "Election created.", roomCode ?? Guid.Empty);
    }
}