using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;

public class SendTwoFactorCodeCommandValidator : AbstractValidator<SendTwoFactorCodeCommand>
{
    public SendTwoFactorCodeCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email shouldn't be empty")
            .EmailAddress()
            .WithMessage("Email has invalid format");
    }
}