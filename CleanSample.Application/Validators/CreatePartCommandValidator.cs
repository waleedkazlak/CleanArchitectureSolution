using CleanSample.Application.Commands.Part;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreatePartCommandValidator : AbstractValidator<CreatePartCommand>
{
    public CreatePartCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Part code is required.")
            .MaximumLength(100).WithMessage("Part code cannot exceed 100 characters.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Part name is required.");

        RuleFor(x => x.NameEn)
            .MaximumLength(200).WithMessage("Part English name cannot exceed 200 characters.");

        RuleFor(x => x.NameAr)
            .MaximumLength(200).WithMessage("Part Arabic name cannot exceed 200 characters.");

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(1000).WithMessage("English description cannot exceed 1000 characters.");

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(1000).WithMessage("Arabic description cannot exceed 1000 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Barcode)
            .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters.");
    }
}
