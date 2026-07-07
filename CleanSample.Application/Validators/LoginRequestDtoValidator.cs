using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

/// <summary>
/// Validator for LoginRequestDto
/// </summary>
public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters long")
            .MaximumLength(50)
            .WithMessage("Username cannot exceed 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long")
            .MaximumLength(100)
            .WithMessage("Password cannot exceed 100 characters");
    }
}