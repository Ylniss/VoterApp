using FluentValidation;
using VoterApp.Application.Common.Constants;

namespace VoterApp.Application.Features.Candidates.Commands.DeleteCandidate;

public class DeleteCandidateCommandValidator : AbstractValidator<DeleteCandidateCommand>
{
    public DeleteCandidateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired);
    }
}