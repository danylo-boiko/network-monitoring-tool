using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.VerifyTwoFactorCode;

public class VerifyTwoFactorCodeCommandValidator : AbstractValidator<VerifyTwoFactorCodeCommand>
{
    public VerifyTwoFactorCodeCommandValidator()
    {
    }
}