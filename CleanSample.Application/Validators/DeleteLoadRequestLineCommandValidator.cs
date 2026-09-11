using CleanSample.Application.Commands.LoadRequestLine;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteLoadRequestLineCommandValidator : AbstractValidator<DeleteLoadRequestLineCommand>
{
    public DeleteLoadRequestLineCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}
