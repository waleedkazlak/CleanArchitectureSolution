using CleanSample.Application.Commands.Material;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateMaterialCommandValidator : AbstractValidator<CreateMaterialCommand>
{
    public CreateMaterialCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Material name is required.")
            .MaximumLength(150).WithMessage("Material name cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
