using CleanSample.Application.Commands.Order;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("Order number is required.")
            .MaximumLength(50).WithMessage("Order number cannot exceed 50 characters.");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0.");

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");

        RuleForEach(x => x.OrderLines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductVariantId)
                .GreaterThan(0).WithMessage("Product variant ID must be greater than 0.");

            line.RuleFor(l => l.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            line.RuleFor(l => l.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        });
    }
}
