using CleanSample.Application.Queries.OrderLine;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetOrderLineByIdQueryValidator : AbstractValidator<GetOrderLineByIdQuery>
{
    public GetOrderLineByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid OrderLine ID.");
    }
}
