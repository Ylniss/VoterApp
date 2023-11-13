using FluentValidation;
using VoterApp.Application.Common.Constants;

namespace VoterApp.Application.Features.Voters.Commands.DeleteVoter;

public class DeleteVoterCommandValidator : AbstractValidator<DeleteVoterCommand>
{
    public DeleteVoterCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired);
    }
}