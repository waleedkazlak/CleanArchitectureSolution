using CleanSample.Application.Commands.Design;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteDesignCommandValidator : AbstractValidator<DeleteDesignCommand>
{
    public DeleteDesignCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid design ID.");
    }
}
