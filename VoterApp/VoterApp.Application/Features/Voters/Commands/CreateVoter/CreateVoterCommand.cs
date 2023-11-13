using MediatR;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Contracts;
using VoterApp.Domain.ValueObjects;

namespace VoterApp.Application.Features.Voters.Commands.CreateVoter;

public record CreateVoterCommand(string Name, int ElectionId, string? KeyPhrase = null) : IRequest<CommandResponse>;

public class CreateVoterCommandHandler : IRequestHandler<CreateVoterCommand, CommandResponse>
{
    private readonly IVoterRepository _voterRepository;

    public CreateVoterCommandHandler(IVoterRepository voterRepository) =>
        _voterRepository = voterRepository;

    public async Task<CommandResponse> Handle(CreateVoterCommand request, CancellationToken cancellationToken)
    {
        request = request with { KeyPhrase = new KeyPhrase().Key };

        var id = await _voterRepository.Create(request);

        return new CommandResponse(id, "Voter created.");
    }
}