using FluentValidation;
using VoterApp.Application.Common.Constants;
using VoterApp.Application.Contracts;

namespace VoterApp.Application.Features.Voters.Commands.Vote;

public class VoteCommandValidator : AbstractValidator<VoteCommand>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IVoterRepository _voterRepository;

    public VoteCommandValidator(IVoterRepository voterRepository, ICandidateRepository candidateRepository)
    {
        _voterRepository = voterRepository;
        _candidateRepository = candidateRepository;

        RuleFor(command => command.VoterId)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired)
            .MustAsync(ExistAndBeInSameElectionAsCandidate)
            .WithMessage(Validation.Messages.MustExistAndBeInSameElectionAsCandidate)
            .MustAsync(HasNotVoted).WithMessage(Validation.Messages.MustHaveNoVote);

        RuleFor(command => command.CandidateId)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired);

        RuleFor(command => command.KeyPhrase)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired)
            .MustAsync(BeEqualToVoterKeyPhrase).WithMessage(Validation.Messages.MustHaveCorrectKeyPhrase);
    }

    private async Task<bool> ExistAndBeInSameElectionAsCandidate(VoteCommand command, int voterId,
        CancellationToken cancellationToken)
    {
        var voter = await _voterRepository.Get(voterId);
        var candidate = await _candidateRepository.Get(command.CandidateId);

        return voter is not null && candidate is not null && voter.Election.Id == candidate.Election.Id;
    }

    private async Task<bool> HasNotVoted(VoteCommand command, int voterId,
        CancellationToken cancellationToken)
    {
        var voter = await _voterRepository.Get(voterId);

        if (voter is null) return true;

        return !voter.HasVoted;
    }

    private async Task<bool> BeEqualToVoterKeyPhrase(VoteCommand command, string keyPhrase,
        CancellationToken cancellationToken)
    {
        var voter = await _voterRepository.Get(command.VoterId);

        if (voter is null) return true;

        return voter.KeyPhrase == keyPhrase;
    }
}