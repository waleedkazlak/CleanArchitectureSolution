using CleanSample.Application.Commands.Design;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateDesignCommandValidator : AbstractValidator<UpdateDesignCommand>
{
    public UpdateDesignCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid design ID.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Design name is required.")
            .MaximumLength(150).WithMessage("Design name cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
