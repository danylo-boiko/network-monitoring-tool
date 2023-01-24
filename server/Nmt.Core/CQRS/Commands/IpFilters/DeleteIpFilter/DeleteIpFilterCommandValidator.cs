using FluentValidation;

namespace Nmt.Core.CQRS.Commands.IpFilters.DeleteIpFilter;

public class DeleteIpFilterCommandValidator : AbstractValidator<DeleteIpFilterCommand>
{
    public DeleteIpFilterCommandValidator()
    {
        RuleFor(c => c.IpFilterId)
            .NotEmpty()
            .WithMessage("IP filter id shouldn't be empty");
    }
}