using CleanSample.Application.Commands.Part;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdatePartCommandValidator : AbstractValidator<UpdatePartCommand>
{
    public UpdatePartCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Part ID must be greater than 0.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Part code is required.")
            .MaximumLength(100).WithMessage("Part code cannot exceed 100 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Part name is required.")
            .MaximumLength(200).WithMessage("Part name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Barcode)
            .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters.");
    }
}
