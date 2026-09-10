using CleanSample.Application.Commands.Load;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteLoadsCommandValidator : AbstractValidator<DeleteLoadsCommand>
{
    public DeleteLoadsCommandValidator()
    {
        RuleFor(x => x.LoadIds)
            .NotNull().WithMessage("LoadIds list is required.")
            .NotEmpty().WithMessage("At least one load ID must be provided.");

        RuleForEach(x => x.LoadIds)
            .GreaterThan(0).WithMessage("Load ID must be greater than 0.");
    }
}
