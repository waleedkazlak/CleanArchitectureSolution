using CleanSample.Application.Commands.VehicleOffload;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteVehicleOffloadCommandValidator : AbstractValidator<DeleteVehicleOffloadCommand>
{
    public DeleteVehicleOffloadCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Vehicle offload ID must be greater than 0.");
    }
}
