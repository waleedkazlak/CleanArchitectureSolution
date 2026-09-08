using CleanSample.Application.Commands.Color;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateColorCommandValidator : AbstractValidator<UpdateColorCommand>
{
    public UpdateColorCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid color ID.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Color name is required.")
            .MaximumLength(100).WithMessage("Color name cannot exceed 100 characters.");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Color code cannot exceed 50 characters.");
    }
}
