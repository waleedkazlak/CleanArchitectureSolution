using CleanSample.Application.Commands.ClientLocation;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteClientLocationCommandValidator : AbstractValidator<DeleteClientLocationCommand>
{
    public DeleteClientLocationCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid Client Location ID.");
    }
}
