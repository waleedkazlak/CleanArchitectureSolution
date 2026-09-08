using CleanSample.Application.Queries.Order;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid Order ID.");
    }
}
