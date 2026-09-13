using CleanSample.Application.Commands.Color;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateColorCommandValidator : AbstractValidator<CreateColorCommand>
{
    public CreateColorCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Color name is required.");

        RuleFor(x => x.NameEn)
            .MaximumLength(100).WithMessage("Color English name cannot exceed 100 characters.");

        RuleFor(x => x.NameAr)
            .MaximumLength(100).WithMessage("Color Arabic name cannot exceed 100 characters.");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Color code cannot exceed 50 characters.");
    }
}
