using CleanSample.Application.Commands.Client;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid client ID.");
    }
}
