using CleanSample.Application.Commands.Color;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteColorCommandValidator : AbstractValidator<DeleteColorCommand>
{
    public DeleteColorCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid color ID.");
    }
}
