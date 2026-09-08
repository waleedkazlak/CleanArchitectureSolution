using CleanSample.Application.Commands.Vehicle;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteVehicleCommandValidator : AbstractValidator<DeleteVehicleCommand>
{
    public DeleteVehicleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid vehicle ID.");
    }
}
