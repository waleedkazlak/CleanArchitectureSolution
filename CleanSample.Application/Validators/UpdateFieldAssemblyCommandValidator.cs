using CleanSample.Application.Commands.FieldAssembly;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateFieldAssemblyCommandValidator : AbstractValidator<UpdateFieldAssemblyCommand>
{
    public UpdateFieldAssemblyCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Field assembly ID must be greater than 0.");

        RuleFor(x => x.FieldJobId)
            .GreaterThan(0).WithMessage("Field job ID must be greater than 0.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(x => x.ProductBarcode)
            .MaximumLength(100).WithMessage("Product barcode cannot exceed 100 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.Status)
            .Must(s => Enum.IsDefined(typeof(CleanSample.Domain.Enums.FieldAssemblyStatusEnum), s))
            .WithMessage("Valid Status is required (1 = InProgress, 2 = Completed, 3 = Cancelled).");

        RuleFor(x => x.TechnicianId)
            .GreaterThan(0).WithMessage("Technician user ID must be greater than 0.")
            .When(x => x.TechnicianId.HasValue);

        RuleFor(x => x.SupervisorId)
            .GreaterThan(0).WithMessage("Supervisor user ID must be greater than 0.")
            .When(x => x.SupervisorId.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");
    }
}
