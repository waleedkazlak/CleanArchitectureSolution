using CleanSample.Application.Commands.Screen;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteScreenCommandValidator : AbstractValidator<DeleteScreenCommand>
{
    public DeleteScreenCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Valid Screen ID is required");
    }
}
