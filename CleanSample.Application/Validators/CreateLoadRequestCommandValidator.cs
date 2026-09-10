using CleanSample.Application.Commands.LoadRequest;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateLoadRequestCommandValidator : AbstractValidator<CreateLoadRequestCommand>
{
    public CreateLoadRequestCommandValidator()
    {
        RuleFor(x => x.RequestNumber)
            .NotEmpty().WithMessage("Request number is required.")
            .MaximumLength(50).WithMessage("Request number cannot exceed 50 characters.");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0.");

        RuleFor(x => x.OrderId)
            .GreaterThan(0).WithMessage("Order ID must be greater than 0.")
            .When(x => x.OrderId.HasValue);

        RuleFor(x => x.ClientLocationId)
            .GreaterThan(0).WithMessage("Client location ID must be greater than 0.")
            .When(x => x.ClientLocationId.HasValue);

        RuleFor(x => x.RequestedBy)
            .GreaterThan(0).WithMessage("RequestedBy user ID must be greater than 0.")
            .When(x => x.RequestedBy.HasValue);

        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Driver ID must be greater than 0.")
            .When(x => x.DriverId.HasValue);

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0.")
            .When(x => x.VehicleId.HasValue);

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

        RuleFor(x => x.DestinationAddress)
            .MaximumLength(500).WithMessage("Destination address cannot exceed 500 characters.");

        RuleFor(x => x.DestinationCity)
            .MaximumLength(150).WithMessage("Destination city cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleForEach(x => x.LoadRequestLines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductVariantId)
                .GreaterThan(0).WithMessage("Product variant ID must be greater than 0.");

            line.RuleFor(l => l.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        });
    }
}
