using CleanSample.Application.Commands.Order;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Order ID must be greater than 0.");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0.");

        RuleFor(x => x.Status)
            .Must(s => Enum.IsDefined(typeof(Domain.Enums.OrderStatusEnum), s))
            .WithMessage("Status must be a valid OrderStatus value.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");

        RuleForEach(x => x.OrderLines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

            line.RuleFor(l => l.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            line.RuleFor(l => l.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        });
    }
}
