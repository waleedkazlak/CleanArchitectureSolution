using CleanSample.Application.Commands.Issue;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class DeleteIssueCommandValidator : AbstractValidator<DeleteIssueCommand>
{
    public DeleteIssueCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Issue ID must be greater than 0.");
    }
}
