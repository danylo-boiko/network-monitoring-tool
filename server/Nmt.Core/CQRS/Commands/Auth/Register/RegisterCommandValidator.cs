using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
    }
}