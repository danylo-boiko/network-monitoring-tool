using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;

public class SendTwoFactorCodeCommandValidator : AbstractValidator<SendTwoFactorCodeCommand>
{
    public SendTwoFactorCodeCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty()
            .WithMessage("Username shouldn't be empty");
    }
}