using CleanSample.Application.Queries.Design;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetDesignByIdQueryValidator : AbstractValidator<GetDesignByIdQuery>
{
    public GetDesignByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid design ID.");
    }
}
