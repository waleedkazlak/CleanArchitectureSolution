using CleanSample.Application.Commands.Category;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid category ID.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Category name is required.");

        RuleFor(x => x.NameEn)
            .MaximumLength(150).WithMessage("Category English name cannot exceed 150 characters.");

        RuleFor(x => x.NameAr)
            .MaximumLength(150).WithMessage("Category Arabic name cannot exceed 150 characters.");

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(500).WithMessage("English description cannot exceed 500 characters.");

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(500).WithMessage("Arabic description cannot exceed 500 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
