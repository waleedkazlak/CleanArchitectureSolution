using CleanSample.Application.Queries.Color;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetColorByIdQueryValidator : AbstractValidator<GetColorByIdQuery>
{
    public GetColorByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid color ID.");
    }
}
