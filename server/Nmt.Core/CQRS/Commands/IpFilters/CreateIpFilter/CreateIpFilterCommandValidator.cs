using FluentValidation;

namespace Nmt.Core.CQRS.Commands.IpFilters.CreateIpFilter;

public class CreateIpFilterCommandValidator : AbstractValidator<CreateIpFilterCommand>
{
    public CreateIpFilterCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User id shouldn't be empty");

        RuleFor(c => c.Ip)
            .NotEmpty()
            .WithMessage("IP shouldn't be empty")
            .Must(ip => ip >= uint.MinValue && ip <= uint.MaxValue)
            .WithMessage("IP should be in unsigned integer range");

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