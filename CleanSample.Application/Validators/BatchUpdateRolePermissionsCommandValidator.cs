using CleanSample.Application.Commands.RolePermission;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class BatchUpdateRolePermissionsCommandValidator : AbstractValidator<BatchUpdateRolePermissionsCommand>
{
    public BatchUpdateRolePermissionsCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull().WithMessage("Request payload is required");

        RuleFor(x => x.Request.RoleId)
            .GreaterThan(0).WithMessage("Valid Role ID is required");

        RuleFor(x => x.Request.Permissions)
            .NotNull().WithMessage("Permissions list is required");
    }
}
