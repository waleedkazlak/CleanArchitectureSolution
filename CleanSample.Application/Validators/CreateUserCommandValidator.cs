using CleanSample.Application.Commands.User;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email address format.")
            .MaximumLength(250).WithMessage("Email cannot exceed 250 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Mobile)
            .MaximumLength(50).WithMessage("Mobile number cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Mobile));

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("RoleId must be greater than 0.")
            .When(x => x.RoleId.HasValue);
    }
}
