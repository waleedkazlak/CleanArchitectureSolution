using CleanSample.Application.Commands.VehicleOffload;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateVehicleOffloadCommandValidator : AbstractValidator<UpdateVehicleOffloadCommand>
{
    public UpdateVehicleOffloadCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Vehicle offload ID must be greater than 0.");

        RuleFor(x => x.LoadRequestId)
            .GreaterThan(0).WithMessage("Load request ID must be greater than 0.");

        RuleFor(x => x.PartId)
            .GreaterThan(0).WithMessage("Part ID must be greater than 0.");

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0.");

        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Driver ID must be greater than 0.");

        RuleFor(x => x.Barcode)
            .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters.");

        RuleFor(x => x.Status)
            .Must(s => !s.HasValue || Enum.IsDefined(typeof(Domain.Enums.VehicleOffloadStatusEnum), s.Value))
            .WithMessage("Status must be a valid VehicleOffloadStatus value.");

        RuleFor(x => x.VerifiedBy)
            .GreaterThan(0).WithMessage("VerifiedBy user ID must be greater than 0.")
            .When(x => x.VerifiedBy.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");
    }
}
