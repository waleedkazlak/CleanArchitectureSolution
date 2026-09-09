using CleanSample.Application.Commands.User;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid user ID.");
    }
}
