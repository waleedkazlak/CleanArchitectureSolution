using CleanSample.Application.Queries.PickRequest;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetPickRequestByIdQueryValidator : AbstractValidator<GetPickRequestByIdQuery>
{
    public GetPickRequestByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid PickRequest ID.");
    }
}
