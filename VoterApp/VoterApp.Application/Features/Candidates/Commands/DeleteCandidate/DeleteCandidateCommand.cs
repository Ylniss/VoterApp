using MediatR;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Candidates.Commands.DeleteCandidate;

public record DeleteCandidateCommand(int Id) : IRequest<CommandResponse>;

public class DeleteCandidateCommandHandler : IRequestHandler<DeleteCandidateCommand, CommandResponse>
{
    private readonly ICandidateRepository _candidateRepository;

    public DeleteCandidateCommandHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<CommandResponse> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.Get(request.Id);

        if (candidate is null)
            throw new NotFoundException(request.Id);

        await _candidateRepository.Delete(request.Id);

        return new CommandResponse(request.Id, "Candidate deleted.");
    }
}