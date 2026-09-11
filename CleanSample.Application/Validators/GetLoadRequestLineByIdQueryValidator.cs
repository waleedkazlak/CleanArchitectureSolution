using CleanSample.Application.Queries.LoadRequestLine;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetLoadRequestLineByIdQueryValidator : AbstractValidator<GetLoadRequestLineByIdQuery>
{
    public GetLoadRequestLineByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}
