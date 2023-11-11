using MediatR;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Candidates.Commands.UpdateCandidate;

public record UpdateCandidateCommand(int Id, string Name) : IRequest<CommandResponse>;

public class UpdateCandidateCommandHandler : IRequestHandler<UpdateCandidateCommand, CommandResponse>
{
    private readonly ICandidateRepository _candidateRepository;

    public UpdateCandidateCommandHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<CommandResponse> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.Get(request.Id);

        if (candidate is null)
            throw new NotFoundException(request.Id);

        await _candidateRepository.Update(request);

        return new CommandResponse(request.Id, "Candidate updated.");
    }
}