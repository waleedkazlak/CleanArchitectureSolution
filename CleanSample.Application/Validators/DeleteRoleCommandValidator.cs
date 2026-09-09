using CleanSample.Application.Commands.Role;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid role ID.");
    }
}
