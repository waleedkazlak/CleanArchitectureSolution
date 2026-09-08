using CleanSample.Application.Commands.OrderLine;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteOrderLineCommandValidator : AbstractValidator<DeleteOrderLineCommand>
{
    public DeleteOrderLineCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid OrderLine ID.");
    }
}
