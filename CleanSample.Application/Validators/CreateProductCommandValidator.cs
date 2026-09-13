using CleanSample.Application.Commands.Product;
using FluentValidation;

namespace CleanSample.Application.Validators;

/// <summary>
/// Validator for CreateProductCommand
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category ID must be greater than 0");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Product name is required");

        RuleFor(x => x.NameEn)
            .MaximumLength(200)
            .WithMessage("Product English name cannot exceed 200 characters");

        RuleFor(x => x.NameAr)
            .MaximumLength(200)
            .WithMessage("Product Arabic name cannot exceed 200 characters");

        RuleFor(x => x.Barcode)
            .MaximumLength(100)
            .WithMessage("Barcode cannot exceed 100 characters");

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(1000)
            .WithMessage("English description cannot exceed 1000 characters");

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(1000)
            .WithMessage("Arabic description cannot exceed 1000 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters");
    }
}