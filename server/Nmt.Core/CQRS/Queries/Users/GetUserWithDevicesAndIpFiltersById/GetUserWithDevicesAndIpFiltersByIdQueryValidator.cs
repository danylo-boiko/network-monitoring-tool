using FluentValidation;

namespace Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;

public class GetUserWithDevicesAndIpFiltersByIdQueryValidator : AbstractValidator<GetUserWithDevicesAndIpFiltersByIdQuery>
{
    public GetUserWithDevicesAndIpFiltersByIdQueryValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User id shouldn't be empty");
    }
}