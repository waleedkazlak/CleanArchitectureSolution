using CleanSample.Application.Commands.OrderLine;
using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateOrderLineItemDtoValidator : AbstractValidator<CreateOrderLineItemDto>
{
    public CreateOrderLineItemDtoValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0).WithMessage("Order ID must be greater than 0.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
    }
}

public class CreateOrderLineCommandValidator : AbstractValidator<CreateOrderLineCommand>
{
    public CreateOrderLineCommandValidator()
    {
        When(x => x.Items != null && x.Items.Any(), () =>
        {
            RuleForEach(x => x.Items)
                .SetValidator(new CreateOrderLineItemDtoValidator());
        })
        .Otherwise(() =>
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Order ID must be greater than 0 when items list is empty.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0 when items list is empty.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0 when items list is empty.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        });
    }
}
