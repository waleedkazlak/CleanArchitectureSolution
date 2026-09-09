using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class IssueSearchFilterDtoValidator : AbstractValidator<IssueSearchFilterDto>
{
    private readonly string[] _validSortFields = { "ReportedAt", "ResolvedAt", "Severity", "Status", "IssueType", "PickRequestId", "FieldJobId", "CreatedAt" };
    private readonly string[] _validSortDirections = { "asc", "desc" };

    public IssueSearchFilterDtoValidator()
    {
        RuleFor(x => x.PageNumber)
            .NotEmpty().WithMessage("Page number is required")
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .NotEmpty().WithMessage("Page size is required")
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(255).WithMessage("Search term cannot exceed 255 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));

        RuleFor(x => x.PickRequestId)
            .GreaterThan(0).WithMessage("Pick request ID must be greater than 0")
            .When(x => x.PickRequestId.HasValue);

        RuleFor(x => x.FieldJobId)
            .GreaterThan(0).WithMessage("Field job ID must be greater than 0")
            .When(x => x.FieldJobId.HasValue);

        RuleFor(x => x.FieldAssemblyId)
            .GreaterThan(0).WithMessage("Field assembly ID must be greater than 0")
            .When(x => x.FieldAssemblyId.HasValue);

        RuleFor(x => x.IssueType)
            .MaximumLength(100).WithMessage("Issue type cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.IssueType));

        RuleFor(x => x.Severity)
            .MaximumLength(50).WithMessage("Severity cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Severity));

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.ReportedBy)
            .GreaterThan(0).WithMessage("ReportedBy user ID must be greater than 0")
            .When(x => x.ReportedBy.HasValue);

        RuleFor(x => x.ResolvedBy)
            .GreaterThan(0).WithMessage("ResolvedBy user ID must be greater than 0")
            .When(x => x.ResolvedBy.HasValue);

        RuleFor(x => x.SortBy)
            .Must(sortBy => _validSortFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Sort field must be one of: {string.Join(", ", _validSortFields)}")
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy));

        RuleFor(x => x.SortDirection)
            .Must(direction => _validSortDirections.Contains(direction?.ToLower() ?? "desc"))
            .WithMessage("Sort direction must be 'asc' or 'desc'")
            .When(x => !string.IsNullOrWhiteSpace(x.SortDirection));
    }
}
