using FluentValidation;
using VoterApp.Application.Common.Constants;

namespace VoterApp.Application.Features.Candidates.Queries.GetCandidate;

public class GetCandidateQueryValidator : AbstractValidator<GetCandidateQuery>
{
    public GetCandidateQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired);
    }
}