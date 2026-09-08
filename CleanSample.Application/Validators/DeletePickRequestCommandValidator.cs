using CleanSample.Application.Commands.PickRequest;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeletePickRequestCommandValidator : AbstractValidator<DeletePickRequestCommand>
{
    public DeletePickRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid PickRequest ID.");
    }
}
