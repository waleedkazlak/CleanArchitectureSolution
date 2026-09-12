using CleanSample.Application.Commands.LoadRequest;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateLoadRequestCommandValidator : AbstractValidator<UpdateLoadRequestCommand>
{
    public UpdateLoadRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Load request ID must be greater than 0.");

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
            .Must(s => Enum.IsDefined(typeof(Domain.Enums.LoadRequestStatusEnum), s))
            .WithMessage("Status must be a valid LoadRequestStatus value.");

        RuleFor(x => x.DestinationAddress)
            .MaximumLength(500).WithMessage("Destination address cannot exceed 500 characters.");

        RuleFor(x => x.DestinationCity)
            .MaximumLength(150).WithMessage("Destination city cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleForEach(x => x.LoadRequestLines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

            line.RuleFor(l => l.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        });
    }
}
