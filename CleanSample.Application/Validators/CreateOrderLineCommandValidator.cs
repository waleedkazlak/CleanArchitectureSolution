using CleanSample.Application.Commands.OrderLine;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateOrderLineCommandValidator : AbstractValidator<CreateOrderLineCommand>
{
    public CreateOrderLineCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0).WithMessage("Order ID must be greater than 0.");

        RuleFor(x => x.ProductVariantId)
            .GreaterThan(0).WithMessage("Product variant ID must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
    }
}
