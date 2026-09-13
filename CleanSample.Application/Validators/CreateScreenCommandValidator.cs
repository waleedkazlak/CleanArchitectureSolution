using CleanSample.Application.Commands.Screen;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateScreenCommandValidator : AbstractValidator<CreateScreenCommand>
{
    public CreateScreenCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Screen name is required");

        RuleFor(x => x.NameEn)
            .MaximumLength(100).WithMessage("Screen English name cannot exceed 100 characters");

        RuleFor(x => x.NameAr)
            .MaximumLength(100).WithMessage("Screen Arabic name cannot exceed 100 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Screen code is required")
            .MaximumLength(50).WithMessage("Screen code cannot exceed 50 characters")
            .Matches(@"^[A-Z0-9_]+$").WithMessage("Screen code must contain only uppercase alphanumeric characters and underscores");

        RuleFor(x => x.Module)
            .MaximumLength(50).WithMessage("Module name cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Module));

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(500).WithMessage("English description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.DescriptionEn));

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(500).WithMessage("Arabic description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.DescriptionAr));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
