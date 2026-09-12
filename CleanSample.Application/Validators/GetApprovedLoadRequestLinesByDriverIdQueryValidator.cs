using CleanSample.Application.Queries.LoadRequestLine;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetApprovedLoadRequestLinesByDriverIdQueryValidator : AbstractValidator<GetApprovedLoadRequestLinesByDriverIdQuery>
{
    public GetApprovedLoadRequestLinesByDriverIdQueryValidator()
    {
        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Driver ID must be greater than 0.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}
