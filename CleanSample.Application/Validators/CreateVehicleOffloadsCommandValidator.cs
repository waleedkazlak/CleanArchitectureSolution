using CleanSample.Application.Commands.VehicleOffload;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateVehicleOffloadsCommandValidator : AbstractValidator<CreateVehicleOffloadsCommand>
{
    public CreateVehicleOffloadsCommandValidator()
    {
        RuleFor(x => x.Offloads)
            .NotNull().WithMessage("Offloads list is required.")
            .NotEmpty().WithMessage("Offloads list must contain at least one item.");

        RuleForEach(x => x.Offloads).ChildRules(offload =>
        {
            offload.RuleFor(p => p.LoadRequestId)
                .GreaterThan(0).WithMessage("Load request ID must be greater than 0.");

            offload.RuleFor(p => p.PartId)
                .GreaterThan(0).WithMessage("Part ID must be greater than 0.");

            offload.RuleFor(p => p.VehicleId)
                .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0.");

            offload.RuleFor(p => p.DriverId)
                .GreaterThan(0).WithMessage("Driver ID must be greater than 0.");

            offload.RuleFor(p => p.Barcode)
                .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters.");

            offload.RuleFor(p => p.VerifiedBy)
                .GreaterThan(0).WithMessage("VerifiedBy user ID must be greater than 0.")
                .When(p => p.VerifiedBy.HasValue);

            offload.RuleFor(p => p.Status)
                .Must(s => !s.HasValue || Enum.IsDefined(typeof(Domain.Enums.VehicleOffloadStatusEnum), s.Value))
                .WithMessage("Status must be a valid VehicleOffloadStatus value.");

            offload.RuleFor(p => p.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        });
    }
}
