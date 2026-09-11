using CleanSample.Application.Commands.ProductBOM;
using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateProductBOMItemDtoValidator : AbstractValidator<CreateProductBOMItemDto>
{
    public CreateProductBOMItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(x => x.PartId)
            .GreaterThan(0).WithMessage("Part ID must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}

public class CreateProductBOMCommandValidator : AbstractValidator<CreateProductBOMCommand>
{
    public CreateProductBOMCommandValidator()
    {
        When(x => x.Items != null && x.Items.Any(), () =>
        {
            RuleForEach(x => x.Items)
                .SetValidator(new CreateProductBOMItemDtoValidator());
        })
        .Otherwise(() =>
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0 when items list is empty.");

            RuleFor(x => x.PartId)
                .GreaterThan(0).WithMessage("Part ID must be greater than 0 when items list is empty.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0 when items list is empty.");
        });
    }
}
