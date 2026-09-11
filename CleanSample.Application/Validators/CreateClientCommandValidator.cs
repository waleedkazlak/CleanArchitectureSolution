using CleanSample.Application.Commands.Client;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Client name is required.")
            .MaximumLength(250).WithMessage("Client name cannot exceed 250 characters.");

        RuleFor(x => x.Phone)
            .MaximumLength(50).WithMessage("Phone cannot exceed 50 characters.");

        RuleFor(x => x.Mobile)
            .MaximumLength(50).WithMessage("Mobile cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .MaximumLength(250).WithMessage("Email cannot exceed 250 characters.")
            .EmailAddress().WithMessage("Invalid email format.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

        RuleFor(x => x.City)
            .MaximumLength(150).WithMessage("City cannot exceed 150 characters.");
    }
}
