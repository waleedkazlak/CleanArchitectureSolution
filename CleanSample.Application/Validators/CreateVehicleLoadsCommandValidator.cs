using CleanSample.Application.Commands.VehicleLoad;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateVehicleLoadsCommandValidator : AbstractValidator<CreateVehicleLoadsCommand>
{
    public CreateVehicleLoadsCommandValidator()
    {
        RuleFor(x => x.Loads)
            .NotNull().WithMessage("Loads list is required.")
            .NotEmpty().WithMessage("Loads list must contain at least one item.");

        RuleForEach(x => x.Loads).ChildRules(load =>
        {
            load.RuleFor(p => p.LoadRequestId)
                .GreaterThan(0).WithMessage("Load request ID must be greater than 0.");

            load.RuleFor(p => p.PartId)
                .GreaterThan(0).WithMessage("Part ID must be greater than 0.");

            load.RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            load.RuleFor(p => p.Barcode)
                .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters.");

            load.RuleFor(p => p.LoadedBy)
                .GreaterThan(0).WithMessage("LoadedBy user ID must be greater than 0.")
                .When(p => p.LoadedBy.HasValue);

            load.RuleFor(p => p.DriverId)
                .GreaterThan(0).WithMessage("Driver ID must be greater than 0.")
                .When(p => p.DriverId.HasValue);

            load.RuleFor(p => p.VehicleId)
                .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0.")
                .When(p => p.VehicleId.HasValue);

            load.RuleFor(p => p.Status)
                .Must(s => !s.HasValue || Enum.IsDefined(typeof(Domain.Enums.VehicleLoadStatusEnum), s.Value))
                .WithMessage("Status must be a valid VehicleLoadStatus value.");

            load.RuleFor(p => p.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        });
    }
}
