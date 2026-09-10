using CleanSample.Application.Commands.VehicleOffload;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateVehicleOffloadCommandValidator : AbstractValidator<CreateVehicleOffloadCommand>
{
    public CreateVehicleOffloadCommandValidator()
    {
        RuleFor(x => x.LoadRequestId)
            .GreaterThan(0).WithMessage("Load request ID must be greater than 0.");

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0.");

        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Driver ID must be greater than 0.");

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

        RuleFor(x => x.VerifiedBy)
            .GreaterThan(0).WithMessage("VerifiedBy user ID must be greater than 0.")
            .When(x => x.VerifiedBy.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");

        RuleForEach(x => x.VehicleOffloadItems).ChildRules(item =>
        {
            item.RuleFor(i => i.PartId)
                .GreaterThan(0).WithMessage("Part ID must be greater than 0.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            item.RuleFor(i => i.LoadId)
                .GreaterThan(0).WithMessage("Load ID must be greater than 0.")
                .When(i => i.LoadId.HasValue);

            item.RuleFor(i => i.Barcode)
                .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters.");
        });
    }
}
