using CleanSample.Application.Commands.FieldAssembly;
using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateFieldAssemblyCommandValidator : AbstractValidator<CreateFieldAssemblyCommand>
{
    public CreateFieldAssemblyCommandValidator()
    {
        When(x => x.Items != null && x.Items.Any(), () =>
        {
            RuleForEach(x => x.Items).SetValidator(new CreateFieldAssemblyItemDtoValidator());
        });

        When(x => x.Items == null || !x.Items.Any(), () =>
        {
            RuleFor(x => x.FieldJobId)
                .GreaterThan(0).WithMessage("Field job ID must be greater than 0.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

            RuleFor(x => x.ProductBarcode)
                .MaximumLength(100).WithMessage("Product barcode cannot exceed 100 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Status)
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

            RuleFor(x => x.TechnicianId)
                .GreaterThan(0).WithMessage("Technician user ID must be greater than 0.")
                .When(x => x.TechnicianId.HasValue);

            RuleFor(x => x.SupervisorId)
                .GreaterThan(0).WithMessage("Supervisor user ID must be greater than 0.")
                .When(x => x.SupervisorId.HasValue);

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");
        });
    }
}

public class CreateFieldAssemblyItemDtoValidator : AbstractValidator<CreateFieldAssemblyItemDto>
{
    public CreateFieldAssemblyItemDtoValidator()
    {
        RuleFor(x => x.FieldJobId)
            .GreaterThan(0).WithMessage("Field job ID must be greater than 0.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(x => x.ProductBarcode)
            .MaximumLength(100).WithMessage("Product barcode cannot exceed 100 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

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
