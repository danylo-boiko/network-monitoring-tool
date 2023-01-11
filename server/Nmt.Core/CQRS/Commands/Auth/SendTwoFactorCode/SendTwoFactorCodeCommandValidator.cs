using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;

public class SendTwoFactorCodeCommandValidator : AbstractValidator<SendTwoFactorCodeCommand>
{
    public SendTwoFactorCodeCommandValidator()
    {
    }
}