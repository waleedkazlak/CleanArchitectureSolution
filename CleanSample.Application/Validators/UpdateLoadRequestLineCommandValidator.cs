using CleanSample.Application.Commands.LoadRequestLine;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateLoadRequestLineCommandValidator : AbstractValidator<UpdateLoadRequestLineCommand>
{
    public UpdateLoadRequestLineCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");

        RuleFor(x => x.LoadRequestId)
            .GreaterThan(0).WithMessage("Load request ID must be greater than 0");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
