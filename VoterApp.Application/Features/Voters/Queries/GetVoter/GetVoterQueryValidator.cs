using FluentValidation;
using VoterApp.Application.Common.Constants;

namespace VoterApp.Application.Features.Voters.Queries.GetVoter;

public class GetVoterQueryValidator : AbstractValidator<GetVoterQuery>
{
    public GetVoterQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Validation.Messages.IsRequired);
    }
}