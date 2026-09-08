using CleanSample.Application.Queries.Vehicle;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetVehicleByIdQueryValidator : AbstractValidator<GetVehicleByIdQuery>
{
    public GetVehicleByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid vehicle ID.");
    }
}
