using CleanSample.Application.Queries.LoadRequest;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetLoadRequestByIdQueryValidator : AbstractValidator<GetLoadRequestByIdQuery>
{
    public GetLoadRequestByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid LoadRequest ID.");
    }
}
