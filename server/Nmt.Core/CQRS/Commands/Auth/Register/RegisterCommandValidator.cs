using FluentValidation;

namespace Nmt.Core.CQRS.Commands.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty()
            .WithMessage("Username shouldn't be empty")
            .MinimumLength(3)
            .WithMessage("Username should has at least 3 characters")
            .MaximumLength(32)
            .WithMessage("Username should has less than 32 characters")
            .Matches("^[A-Za-z0-9_]+$")
            .WithMessage("Username should has latin letters, numbers and underscores");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email shouldn't be empty")
            .EmailAddress()
            .WithMessage("Email has invalid format");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password shouldn't be empty")
            .MinimumLength(8)
            .WithMessage("Password should has at least 8 characters")
            .MaximumLength(128)
            .WithMessage("Password should has less than 128 characters")
            .Matches(@"^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9\s!@#$%^&*()_+=-`~\\\]\[{}|';:/.,?><]+)$")
            .WithMessage("Password must contains at least one letter and one digit");

        RuleFor(c => c.ConfirmPassword)
            .Equal(c => c.Password)
            .WithMessage("Confirm password don't match with password");
    }
}