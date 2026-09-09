using CleanSample.Application.Commands.FieldJob;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateFieldJobCommandValidator : AbstractValidator<CreateFieldJobCommand>
{
    public CreateFieldJobCommandValidator()
    {
        RuleFor(x => x.JobNumber)
            .NotEmpty().WithMessage("Job number is required.")
            .MaximumLength(50).WithMessage("Job number cannot exceed 50 characters.");

        RuleFor(x => x.PickRequestId)
            .GreaterThan(0).WithMessage("Pick request ID must be greater than 0.");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0.");

        RuleFor(x => x.ClientLocationId)
            .GreaterThan(0).WithMessage("Client location ID must be greater than 0.")
            .When(x => x.ClientLocationId.HasValue);

        RuleFor(x => x.TechnicianId)
            .GreaterThan(0).WithMessage("Technician user ID must be greater than 0.")
            .When(x => x.TechnicianId.HasValue);

        RuleFor(x => x.SupervisorId)
            .GreaterThan(0).WithMessage("Supervisor user ID must be greater than 0.")
            .When(x => x.SupervisorId.HasValue);

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");
    }
}
