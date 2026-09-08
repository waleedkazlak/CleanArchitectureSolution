using CleanSample.Application.Commands.ProductVariant;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateProductVariantCommandValidator : AbstractValidator<CreateProductVariantCommand>
{
    public CreateProductVariantCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Valid Product ID is required.");

        RuleFor(x => x.ColorId)
            .GreaterThan(0).WithMessage("Color ID must be greater than 0.")
            .When(x => x.ColorId.HasValue);

        RuleFor(x => x.MaterialId)
            .GreaterThan(0).WithMessage("Material ID must be greater than 0.")
            .When(x => x.MaterialId.HasValue);

        RuleFor(x => x.DesignId)
            .GreaterThan(0).WithMessage("Design ID must be greater than 0.")
            .When(x => x.DesignId.HasValue);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Variant code is required.")
            .MaximumLength(100).WithMessage("Variant code cannot exceed 100 characters.");

        RuleFor(x => x.Barcode)
            .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
    }
}
