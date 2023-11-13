using FluentValidation;
using VoterApp.Application.Common.Constants;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Elections.Commands.CreateElection;

public class CreateElectionCommandValidator : AbstractValidator<CreateElectionCommand>
{
    private readonly IElectionRepository _electionRepository;

    public CreateElectionCommandValidator(IElectionRepository electionRepository)
    {
        _electionRepository = electionRepository;

        RuleFor(command => command.Topic)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired)
            .MinimumLength(Validation.MinTopicLength).WithMessage(Validation.Messages.MinTopicLength)
            .MaximumLength(Validation.MaxTopicLength).WithMessage(Validation.Messages.MaxTopicLength);
    }
}