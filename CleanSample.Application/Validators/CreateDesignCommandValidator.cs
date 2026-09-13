using CleanSample.Application.Commands.Design;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class CreateDesignCommandValidator : AbstractValidator<CreateDesignCommand>
{
    public CreateDesignCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Design name is required.");

        RuleFor(x => x.NameEn)
            .MaximumLength(150).WithMessage("Design English name cannot exceed 150 characters.");

        RuleFor(x => x.NameAr)
            .MaximumLength(150).WithMessage("Design Arabic name cannot exceed 150 characters.");

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(500).WithMessage("English description cannot exceed 500 characters.");

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(500).WithMessage("Arabic description cannot exceed 500 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
