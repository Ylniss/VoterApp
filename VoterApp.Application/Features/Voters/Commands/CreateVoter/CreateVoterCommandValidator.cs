using FluentValidation;
using VoterApp.Application.Common.Constants;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Voters.Commands.CreateVoter;

public class CreateVoterCommandValidator : AbstractValidator<CreateVoterCommand>
{
    private readonly IElectionRepository _electionRepository;
    private readonly IVoterRepository _voterRepository;

    public CreateVoterCommandValidator(IVoterRepository voterRepository,
        IElectionRepository electionRepository)
    {
        _voterRepository = voterRepository;
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

    private async Task<bool> BeUniqueNameInElection(CreateVoterCommand command, string name,
        CancellationToken cancellationToken)
    {
        var voters = await _voterRepository.GetAll();
        return voters.Where(c => c.Election.Id == command.ElectionId)
            .All(c => c.Name != name);
    }
}