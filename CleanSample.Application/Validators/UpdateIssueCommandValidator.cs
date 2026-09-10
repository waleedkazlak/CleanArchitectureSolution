using CleanSample.Application.Commands.Issue;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class UpdateIssueCommandValidator : AbstractValidator<UpdateIssueCommand>
{
    public UpdateIssueCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Issue ID must be greater than 0.");

        RuleFor(x => x.LoadRequestId)
            .GreaterThan(0).WithMessage("Load request ID must be greater than 0.")
            .When(x => x.LoadRequestId.HasValue);

        RuleFor(x => x.FieldJobId)
            .GreaterThan(0).WithMessage("Field job ID must be greater than 0.")
            .When(x => x.FieldJobId.HasValue);

        RuleFor(x => x.FieldAssemblyId)
            .GreaterThan(0).WithMessage("Field assembly ID must be greater than 0.")
            .When(x => x.FieldAssemblyId.HasValue);

        RuleFor(x => x.IssueType)
            .NotEmpty().WithMessage("Issue type is required.")
            .MaximumLength(100).WithMessage("Issue type cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

        RuleFor(x => x.Severity)
            .MaximumLength(50).WithMessage("Severity cannot exceed 50 characters.");

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

        RuleFor(x => x.ReportedBy)
            .GreaterThan(0).WithMessage("ReportedBy user ID must be greater than 0.")
            .When(x => x.ReportedBy.HasValue);

        RuleFor(x => x.ResolvedBy)
            .GreaterThan(0).WithMessage("ResolvedBy user ID must be greater than 0.")
            .When(x => x.ResolvedBy.HasValue);

        RuleFor(x => x.ResolutionNotes)
            .MaximumLength(2000).WithMessage("Resolution notes cannot exceed 2000 characters.");
    }
}
