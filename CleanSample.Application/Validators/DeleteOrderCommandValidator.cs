using CleanSample.Application.Commands.Order;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid Order ID.");
    }
}
