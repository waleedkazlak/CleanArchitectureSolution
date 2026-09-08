using CleanSample.Application.Commands.Design;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateDesignCommandValidator : AbstractValidator<CreateDesignCommand>
{
    public CreateDesignCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Design name is required.")
            .MaximumLength(150).WithMessage("Design name cannot exceed 150 characters.");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Design code cannot exceed 50 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
