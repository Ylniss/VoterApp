using FluentValidation;
using VoterApp.Application.Common.Constants;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Candidates.Commands.CreateCandidate;

public class CreateCandidateCommandValidator : AbstractValidator<CreateCandidateCommand>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IElectionRepository _electionRepository;

    public CreateCandidateCommandValidator(ICandidateRepository candidateRepository,
        IElectionRepository electionRepository)
    {
        _candidateRepository = candidateRepository;
        _electionRepository = electionRepository;

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired)
            .MinimumLength(Validation.MinNameLength).WithMessage(Validation.Messages.MinNameLength)
            .MaximumLength(Validation.MaxNameLength).WithMessage(Validation.Messages.MaxNameLength)
            .MustAsync(BeUniqueNameInElection).WithMessage(Validation.Messages.MustBeUniqueInElection);

        RuleFor(command => command.ElectionId)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired)
            .MustAsync(ExistElection).WithMessage(Validation.Messages.MustExistElection);
    }

    private async Task<bool> ExistElection(int entityId,
        CancellationToken cancellationToken)
    {
        var election = await _electionRepository.Get(entityId);
        return election != null;
    }

    private async Task<bool> BeUniqueNameInElection(CreateCandidateCommand command, string name,
        CancellationToken cancellationToken)
    {
        var candidates = await _candidateRepository.GetAll();
        return candidates.Where(c => c.Election.Id == command.ElectionId)
            .All(c => c.Name != name);
    }
}