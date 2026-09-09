using CleanSample.Application.Commands.Screen;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateScreenCommandValidator : AbstractValidator<UpdateScreenCommand>
{
    public UpdateScreenCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Valid Screen ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Screen name is required")
            .MaximumLength(100).WithMessage("Screen name cannot exceed 100 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Screen code is required")
            .MaximumLength(50).WithMessage("Screen code cannot exceed 50 characters")
            .Matches(@"^[A-Z0-9_]+$").WithMessage("Screen code must contain only uppercase alphanumeric characters and underscores");

        RuleFor(x => x.Module)
            .MaximumLength(50).WithMessage("Module name cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Module));

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Description cannot exceed 250 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
