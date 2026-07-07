using CleanSample.Application.Commands.Product;
using FluentValidation;

namespace CleanSample.Application.Validators;

/// <summary>
/// Validator for UpdateProductCommand
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product id is required")
            .GreaterThan(0)
            .WithMessage("Product id must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(255)
            .WithMessage("Product name cannot exceed 255 characters")
            .MinimumLength(3)
            .WithMessage("Product name must be at least 3 characters long");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Product description is required")
            .MaximumLength(1000)
            .WithMessage("Product description cannot exceed 1000 characters")
            .MinimumLength(10)
            .WithMessage("Product description must be at least 10 characters long");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Product price is required")
            .GreaterThan(0)
            .WithMessage("Product price must be greater than 0")
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Product price cannot exceed 999,999.99");

        RuleFor(x => x.Stock)
            .NotEmpty()
            .WithMessage("Product stock is required")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Product stock cannot be negative")
            .LessThanOrEqualTo(1000000)
            .WithMessage("Product stock cannot exceed 1,000,000");

        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage("Product active status is required");
    }
}