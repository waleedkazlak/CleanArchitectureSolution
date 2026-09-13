using CleanSample.Application.Commands.Role;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid role ID.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Role name is required.");

        RuleFor(x => x.NameEn)
            .MaximumLength(100).WithMessage("Role English name cannot exceed 100 characters.");

        RuleFor(x => x.NameAr)
            .MaximumLength(100).WithMessage("Role Arabic name cannot exceed 100 characters.");

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(500).WithMessage("English description cannot exceed 500 characters.");

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(500).WithMessage("Arabic description cannot exceed 500 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
