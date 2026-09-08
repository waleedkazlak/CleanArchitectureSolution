using CleanSample.Application.Commands.ProductBOM;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteProductBOMCommandValidator : AbstractValidator<DeleteProductBOMCommand>
{
    public DeleteProductBOMCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid Product BOM ID.");
    }
}
