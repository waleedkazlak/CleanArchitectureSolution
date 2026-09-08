using CleanSample.Application.Commands.ProductVariant;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteProductVariantCommandValidator : AbstractValidator<DeleteProductVariantCommand>
{
    public DeleteProductVariantCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid variant ID.");
    }
}
