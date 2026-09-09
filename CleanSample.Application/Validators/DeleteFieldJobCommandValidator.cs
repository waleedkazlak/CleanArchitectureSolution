using CleanSample.Application.Commands.FieldJob;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteFieldJobCommandValidator : AbstractValidator<DeleteFieldJobCommand>
{
    public DeleteFieldJobCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Field job ID must be greater than 0.");
    }
}
