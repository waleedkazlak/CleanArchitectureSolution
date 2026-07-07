using CleanSample.Application.Commands.Product;
using FluentValidation;

namespace CleanSample.Application.Validators;

/// <summary>
/// Validator for DeleteProductCommand
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product id is required")
            .GreaterThan(0)
            .WithMessage("Product id must be greater than 0");
    }
}