using MediatR;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Candidates.Commands.CreateCandidate;

public record CreateCandidateCommand(string Name, int ElectionId) : IRequest<CommandResponse>;

public class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, CommandResponse>
{
    private readonly ICandidateRepository _candidateRepository;

    public CreateCandidateCommandHandler(ICandidateRepository candidateRepository) =>
        _candidateRepository = candidateRepository;

    public async Task<CommandResponse> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        var id = await _candidateRepository.Create(request);

        return new CommandResponse(id, "Candidate created.");
    }
}