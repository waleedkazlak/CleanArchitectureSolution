using CleanSample.Application.Queries.LoadRequestPart;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetLoadRequestPartsByLoadRequestIdQueryValidator : AbstractValidator<GetLoadRequestPartsByLoadRequestIdQuery>
{
    public GetLoadRequestPartsByLoadRequestIdQueryValidator()
    {
        RuleFor(x => x.LoadRequestId)
            .GreaterThan(0).WithMessage("LoadRequestId must be greater than 0.");
    }
}
