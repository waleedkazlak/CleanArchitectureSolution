using CleanSample.Application.Commands.Color;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateColorCommandValidator : AbstractValidator<CreateColorCommand>
{
    public CreateColorCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Color name is required.")
            .MaximumLength(100).WithMessage("Color name cannot exceed 100 characters.");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Color code cannot exceed 50 characters.");
    }
}
