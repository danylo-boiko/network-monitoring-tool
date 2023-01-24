using FluentValidation;

namespace Nmt.Core.CQRS.Queries.IpFilters.GetIpFiltersByUserId;

public class GetIpFiltersByUserIdQueryValidator : AbstractValidator<GetIpFiltersByUserIdQuery>
{
    public GetIpFiltersByUserIdQueryValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User id shouldn't be empty");
    }
}