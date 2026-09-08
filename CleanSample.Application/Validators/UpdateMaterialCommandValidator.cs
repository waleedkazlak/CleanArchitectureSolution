using CleanSample.Application.Commands.Material;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateMaterialCommandValidator : AbstractValidator<UpdateMaterialCommand>
{
    public UpdateMaterialCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid material ID.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Material name is required.")
            .MaximumLength(150).WithMessage("Material name cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
