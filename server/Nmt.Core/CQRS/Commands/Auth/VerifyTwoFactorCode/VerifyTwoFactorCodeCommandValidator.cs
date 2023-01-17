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
            .Matches("^[0-9]{6}$")
            .WithMessage("Two factor code should contains only 6 digits");
    }
}