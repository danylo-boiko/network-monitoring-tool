using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty()
            .WithMessage("Username shouldn't be empty")
            .MinimumLength(3)
            .WithMessage("Username should has at least 3 characters")
            .MaximumLength(32)
            .WithMessage("Username should has less than 32 characters")
            .Matches("^[A-Za-z0-9_-]+$")
            .WithMessage("Username should has only latin letters, numbers, dashes and underscores");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password shouldn't be empty")
            .MinimumLength(8)
            .WithMessage("Password should has at least 8 characters")
            .Matches(@"^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9\s!@#$%^&*()_+=-`~\\\]\[{}|';:/.,?><]+)$")
            .WithMessage("Password must contains at least one letter and one number");
    }
}