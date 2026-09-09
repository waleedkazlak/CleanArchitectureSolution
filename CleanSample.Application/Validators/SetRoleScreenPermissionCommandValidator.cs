using CleanSample.Application.Commands.RolePermission;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class SetRoleScreenPermissionCommandValidator : AbstractValidator<SetRoleScreenPermissionCommand>
{
    public SetRoleScreenPermissionCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("Valid Role ID is required");

        RuleFor(x => x.ScreenId)
            .GreaterThan(0).WithMessage("Valid Screen ID is required");
    }
}
