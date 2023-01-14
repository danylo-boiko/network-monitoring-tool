using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.VerifyTwoFactorCode;

public class VerifyTwoFactorCodeCommandValidator : AbstractValidator<VerifyTwoFactorCodeCommand>
{
    public VerifyTwoFactorCodeCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty()
            .WithMessage("Username shouldn't be empty");

        RuleFor(c => c.TwoFactorCode)
            .NotEmpty()
            .WithMessage("Two factor code shouldn't be empty")
            .Length(6, 6)
            .WithMessage("Two factor code should has 6 numbers")
            .Matches("^[0-9]+$")
            .WithMessage("Two factor code should contains only numbers");
    }
}