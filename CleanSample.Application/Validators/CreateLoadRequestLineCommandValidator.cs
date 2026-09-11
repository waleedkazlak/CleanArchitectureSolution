using CleanSample.Application.Commands.LoadRequestLine;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateLoadRequestLineCommandValidator : AbstractValidator<CreateLoadRequestLineCommand>
{
    public CreateLoadRequestLineCommandValidator()
    {
        When(x => x.Items != null && x.Items.Any(), () =>
        {
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.LoadRequestId)
                    .GreaterThan(0).WithMessage("Load request ID must be greater than 0");

                item.RuleFor(x => x.ProductId)
                    .GreaterThan(0).WithMessage("Product ID must be greater than 0");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0");
            });
        }).Otherwise(() =>
        {
            RuleFor(x => x.LoadRequestId)
                .GreaterThan(0).WithMessage("Load request ID must be greater than 0");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        });
    }
}
