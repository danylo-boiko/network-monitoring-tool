using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(c => c.AccessToken)
            .NotEmpty()
            .WithMessage("Access token shouldn't be empty");

        RuleFor(c => c.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token shouldn't be empty");
    }
}