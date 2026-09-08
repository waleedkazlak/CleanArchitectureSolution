using CleanSample.Application.Commands.Vehicle;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.VehicleNumber)
            .NotEmpty().WithMessage("Vehicle number is required.")
            .MaximumLength(50).WithMessage("Vehicle number cannot exceed 50 characters.");

        RuleFor(x => x.PlateNumber)
            .NotEmpty().WithMessage("Plate number is required.")
            .MaximumLength(50).WithMessage("Plate number cannot exceed 50 characters.");

        RuleFor(x => x.VehicleType)
            .MaximumLength(100).WithMessage("Vehicle type cannot exceed 100 characters.");

        RuleFor(x => x.CapacityKg)
            .GreaterThan(0).When(x => x.CapacityKg.HasValue).WithMessage("Capacity must be greater than 0.");
    }
}
