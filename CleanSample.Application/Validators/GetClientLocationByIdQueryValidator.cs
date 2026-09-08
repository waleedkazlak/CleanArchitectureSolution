using CleanSample.Application.Queries.ClientLocation;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetClientLocationByIdQueryValidator : AbstractValidator<GetClientLocationByIdQuery>
{
    public GetClientLocationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid Client Location ID.");
    }
}
