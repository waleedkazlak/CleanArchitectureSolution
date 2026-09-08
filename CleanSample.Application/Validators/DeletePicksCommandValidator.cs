using CleanSample.Application.Commands.Pick;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeletePicksCommandValidator : AbstractValidator<DeletePicksCommand>
{
    public DeletePicksCommandValidator()
    {
        RuleFor(x => x.PickIds)
            .NotNull().WithMessage("PickIds list is required.")
            .NotEmpty().WithMessage("At least one pick ID must be provided.");

        RuleForEach(x => x.PickIds)
            .GreaterThan(0).WithMessage("Pick ID must be greater than 0.");
    }
}
