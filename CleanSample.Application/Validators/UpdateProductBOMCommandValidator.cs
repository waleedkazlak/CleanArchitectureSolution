using CleanSample.Application.Commands.ProductBOM;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateProductBOMCommandValidator : AbstractValidator<UpdateProductBOMCommand>
{
    public UpdateProductBOMCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Product BOM ID must be greater than 0.");

        RuleFor(x => x.ProductVariantId)
            .GreaterThan(0).WithMessage("Product Variant ID must be greater than 0.");

        RuleFor(x => x.PartId)
            .GreaterThan(0).WithMessage("Part ID must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
