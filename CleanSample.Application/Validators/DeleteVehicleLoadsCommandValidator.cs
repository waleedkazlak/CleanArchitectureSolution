using CleanSample.Application.Commands.VehicleLoad;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteVehicleLoadsCommandValidator : AbstractValidator<DeleteVehicleLoadsCommand>
{
    public DeleteVehicleLoadsCommandValidator()
    {
        RuleFor(x => x.LoadIds)
            .NotNull().WithMessage("LoadIds list is required.")
            .NotEmpty().WithMessage("At least one load ID must be provided.");

        RuleForEach(x => x.LoadIds)
            .GreaterThan(0).WithMessage("Load ID must be greater than 0.");
    }
}
