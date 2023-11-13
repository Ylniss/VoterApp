using FluentValidation;
using VoterApp.Application.Common.Constants;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Voters.Commands.UpdateVoterName;

public class UpdateVoterCommandValidator : AbstractValidator<UpdateVoterCommand>
{
    private readonly IVoterRepository _voterRepository;

    public UpdateVoterCommandValidator(IVoterRepository voterRepository)
    {
        _voterRepository = voterRepository;

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired)
            .MinimumLength(Validation.MinNameLength).WithMessage(Validation.Messages.MinNameLength)
            .MaximumLength(Validation.MaxNameLength).WithMessage(Validation.Messages.MaxNameLength)
            .MustAsync(BeUniqueNameInElection).WithMessage(Validation.Messages.MustBeUniqueInElection);
    }

    private async Task<bool> BeUniqueNameInElection(UpdateVoterCommand command, string name,
        CancellationToken cancellationToken)
    {
        var voters = await _voterRepository.GetAll();
        return voters.Where(c => c.Election.Id == command.ElectionId)
            .All(c => c.Name != name);
    }
}