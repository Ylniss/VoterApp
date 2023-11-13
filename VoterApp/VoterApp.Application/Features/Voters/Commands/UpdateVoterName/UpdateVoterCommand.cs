using MediatR;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Voters.Commands.UpdateVoterName;

public record UpdateVoterCommand : IRequest<CommandResponse>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public int? VotedCandidateId { get; init; } = null;
    public int ElectionId { get; init; }
}

public class UpdateVoterCommandHandler : IRequestHandler<UpdateVoterCommand, CommandResponse>
{
    private readonly IVoterRepository _voterRepository;

    public UpdateVoterCommandHandler(IVoterRepository voterRepository) =>
        _voterRepository = voterRepository;

    public async Task<CommandResponse> Handle(UpdateVoterCommand request, CancellationToken cancellationToken)
    {
        var voter = await _voterRepository.Get(request.Id);

        if (voter is null)
            throw new NotFoundException(request.Id);

        await _voterRepository.Update(request);

        return new CommandResponse(request.Id, "Voter updated.");
    }
}