using CleanSample.Application.Commands.LoadRequest;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteLoadRequestCommandValidator : AbstractValidator<DeleteLoadRequestCommand>
{
    public DeleteLoadRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Load request ID must be greater than 0.");
    }
}
