using FluentValidation;
using VoterApp.Application.Common.Constants;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Candidates.Commands.CreateCandidate;

public class CreateCandidateCommandValidator : AbstractValidator<CreateCandidateCommand>
{
    private readonly ICandidateRepository _candidateRepository;

    public CreateCandidateCommandValidator(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired)
            .MinimumLength(Validation.MinNameLength).WithMessage(Validation.Messages.MinLength)
            .MaximumLength(Validation.MaxNameLength).WithMessage(Validation.Messages.MaxLength)
            .MustAsync(BeUniqueName).WithMessage(Validation.Messages.MustBeUnique);
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var candidates = await _candidateRepository.GetAll();
        return candidates.All(x => x.Name != name);
    }
}