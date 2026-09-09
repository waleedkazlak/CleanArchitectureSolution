using CleanSample.Application.Commands.VehicleLoad;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateVehicleLoadCommandValidator : AbstractValidator<UpdateVehicleLoadCommand>
{
    public UpdateVehicleLoadCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Vehicle load ID must be greater than 0.");

        RuleFor(x => x.PickRequestId)
            .GreaterThan(0).WithMessage("Pick request ID must be greater than 0.");

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

        RuleForEach(x => x.VehicleLoadItems).ChildRules(item =>
        {
            item.RuleFor(i => i.PartId)
                .GreaterThan(0).WithMessage("Part ID must be greater than 0.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            item.RuleFor(i => i.PickId)
                .GreaterThan(0).WithMessage("Pick ID must be greater than 0.")
                .When(i => i.PickId.HasValue);

            item.RuleFor(i => i.Barcode)
                .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters.");
        });
    }
}
