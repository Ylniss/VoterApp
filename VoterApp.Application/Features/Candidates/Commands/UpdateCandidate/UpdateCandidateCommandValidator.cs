using FluentValidation;
using VoterApp.Application.Common.Constants;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Candidates.Commands.UpdateCandidate;

public class UpdateCandidateCommandValidator : AbstractValidator<UpdateCandidateCommand>
{
    private readonly ICandidateRepository _candidateRepository;

    public UpdateCandidateCommandValidator(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired)
            .MinimumLength(Validation.MinNameLength).WithMessage(Validation.Messages.MinNameLength)
            .MaximumLength(Validation.MaxNameLength).WithMessage(Validation.Messages.MaxNameLength)
            .MustAsync(BeUniqueNameInElection).WithMessage(Validation.Messages.MustBeUniqueInElection);
    }

    private async Task<bool> BeUniqueNameInElection(UpdateCandidateCommand command, string name,
        CancellationToken cancellationToken)
    {
        var candidates = await _candidateRepository.GetAll();
        return candidates.Where(c => c.Election.Id == command.ElectionId)
            .All(c => c.Name != name);
    }
}