using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
    }
}