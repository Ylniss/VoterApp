using MediatR;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Voters.Commands.DeleteVoter;

public record DeleteVoterCommand(int Id) : IRequest<CommandResponse>;

public class DeleteVoterCommandHandler : IRequestHandler<DeleteVoterCommand, CommandResponse>
{
    private readonly IVoterRepository _voterRepository;

    public DeleteVoterCommandHandler(IVoterRepository voterRepository) => _voterRepository = voterRepository;

    public async Task<CommandResponse> Handle(DeleteVoterCommand request, CancellationToken cancellationToken)
    {
        var voter = await _voterRepository.Get(request.Id);

        if (voter is null)
            throw new NotFoundException(request.Id);

        await _voterRepository.Delete(request.Id);

        return new CommandResponse(request.Id, "Voter deleted.");
    }
}