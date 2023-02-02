using FluentValidation;

namespace Nmt.Core.CQRS.Commands.IpFilters.UpdateIpFilter;

public class UpdateIpFilterCommandValidator : AbstractValidator<UpdateIpFilterCommand>
{
    public UpdateIpFilterCommandValidator()
    {
        RuleFor(c => c.IpFilterId)
            .NotEmpty()
            .WithMessage("IP filter id shouldn't be empty");
        
        RuleFor(c => c.FilterAction)
            .NotEmpty()
            .WithMessage("Filter action shouldn't be empty")
            .IsInEnum()
            .WithMessage("Filter action should be in enum range");

        RuleFor(c => c.Comment)
            .MaximumLength(256)
            .WithMessage("Comment should has less than 256 characters");
    }   
}