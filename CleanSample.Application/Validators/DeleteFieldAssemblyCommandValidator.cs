using CleanSample.Application.Commands.FieldAssembly;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteFieldAssemblyCommandValidator : AbstractValidator<DeleteFieldAssemblyCommand>
{
    public DeleteFieldAssemblyCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Field assembly ID must be greater than 0.");
    }
}
