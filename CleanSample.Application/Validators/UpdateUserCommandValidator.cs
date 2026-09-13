using CleanSample.Application.Commands.User;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid user ID.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.FullNameEn) || !string.IsNullOrWhiteSpace(x.FullName))
            .WithMessage("Full name is required.");

        RuleFor(x => x.FullNameEn)
            .MaximumLength(200).WithMessage("Full name (English) cannot exceed 200 characters.");

        RuleFor(x => x.FullNameAr)
            .MaximumLength(200).WithMessage("Full name (Arabic) cannot exceed 200 characters.");

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

        RuleFor(x => x.PreferredLanguage)
            .Must(lang => string.IsNullOrWhiteSpace(lang) || lang.ToLower() is "en" or "ar")
            .WithMessage("PreferredLanguage must be 'en' or 'ar'.")
            .When(x => !string.IsNullOrWhiteSpace(x.PreferredLanguage));
    }
}
