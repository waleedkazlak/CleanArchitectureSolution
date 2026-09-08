using CleanSample.Application.Queries.Part;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetPartByIdQueryValidator : AbstractValidator<GetPartByIdQuery>
{
    public GetPartByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid part ID.");
    }
}
