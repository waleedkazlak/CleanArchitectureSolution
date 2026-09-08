using CleanSample.Application.Commands.Part;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeletePartCommandValidator : AbstractValidator<DeletePartCommand>
{
    public DeletePartCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid part ID.");
    }
}
