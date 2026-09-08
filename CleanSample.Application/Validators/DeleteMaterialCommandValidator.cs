using CleanSample.Application.Commands.Material;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteMaterialCommandValidator : AbstractValidator<DeleteMaterialCommand>
{
    public DeleteMaterialCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid material ID.");
    }
}
