using MediatR;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Elections.Commands.CreateElection;

public record CreateElectionCommand(string Topic) : IRequest<CommandResponse>;

public class CreateElectionCommandHandler : IRequestHandler<CreateElectionCommand, CommandResponse>
{
    private readonly IElectionRepository _electionRepository;

    public CreateElectionCommandHandler(IElectionRepository electionRepository) =>
        _electionRepository = electionRepository;

    public async Task<CommandResponse> Handle(CreateElectionCommand request,
        CancellationToken cancellationToken)
    {
        var id = await _electionRepository.Create(request);

        return new CommandResponse(id, "Election created.");
    }
}