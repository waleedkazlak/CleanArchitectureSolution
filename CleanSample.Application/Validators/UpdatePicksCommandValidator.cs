using CleanSample.Application.Commands.Pick;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdatePicksCommandValidator : AbstractValidator<UpdatePicksCommand>
{
    public UpdatePicksCommandValidator()
    {
        RuleFor(x => x.Picks)
            .NotNull().WithMessage("Picks list is required.")
            .NotEmpty().WithMessage("Picks list must contain at least one item.");

        RuleForEach(x => x.Picks).ChildRules(pick =>
        {
            pick.RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("Pick ID must be greater than 0.");

            pick.RuleFor(p => p.PickRequestId)
                .GreaterThan(0).WithMessage("Pick request ID must be greater than 0.");

            pick.RuleFor(p => p.PartId)
                .GreaterThan(0).WithMessage("Part ID must be greater than 0.");

            pick.RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            pick.RuleFor(p => p.Barcode)
                .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters.");

            pick.RuleFor(p => p.PickedBy)
                .GreaterThan(0).WithMessage("PickedBy user ID must be greater than 0.")
                .When(p => p.PickedBy.HasValue);

            pick.RuleFor(p => p.DriverId)
                .GreaterThan(0).WithMessage("Driver ID must be greater than 0.")
                .When(p => p.DriverId.HasValue);

            pick.RuleFor(p => p.VehicleId)
                .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0.")
                .When(p => p.VehicleId.HasValue);

            pick.RuleFor(p => p.Status)
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

            pick.RuleFor(p => p.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        });
    }
}
