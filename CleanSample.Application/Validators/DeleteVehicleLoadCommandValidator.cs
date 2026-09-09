using CleanSample.Application.Commands.VehicleLoad;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteVehicleLoadCommandValidator : AbstractValidator<DeleteVehicleLoadCommand>
{
    public DeleteVehicleLoadCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Vehicle load ID must be greater than 0.");
    }
}
