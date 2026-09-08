using CleanSample.Application.Commands.ClientLocation;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateClientLocationCommandValidator : AbstractValidator<UpdateClientLocationCommand>
{
    public UpdateClientLocationCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Client Location ID must be greater than 0.");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Location name is required.")
            .MaximumLength(200).WithMessage("Location name cannot exceed 200 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

        RuleFor(x => x.City)
            .MaximumLength(150).WithMessage("City cannot exceed 150 characters.");

        RuleFor(x => x.ContactName)
            .MaximumLength(200).WithMessage("Contact name cannot exceed 200 characters.");

        RuleFor(x => x.ContactPhone)
            .MaximumLength(50).WithMessage("Contact phone cannot exceed 50 characters.");
    }
}
