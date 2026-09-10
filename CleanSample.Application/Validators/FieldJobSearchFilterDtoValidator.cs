using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class FieldJobSearchFilterDtoValidator : AbstractValidator<FieldJobSearchFilterDto>
{
    private readonly string[] _validSortFields = { "JobNumber", "ScheduledDate", "StartDate", "CompletionDate", "LoadRequestId", "ClientId", "Status", "CreatedAt" };
    private readonly string[] _validSortDirections = { "asc", "desc" };

    public FieldJobSearchFilterDtoValidator()
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

        RuleFor(x => x.JobNumber)
            .MaximumLength(50).WithMessage("Job number cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.JobNumber));

        RuleFor(x => x.LoadRequestId)
            .GreaterThan(0).WithMessage("Load request ID must be greater than 0")
            .When(x => x.LoadRequestId.HasValue);

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0")
            .When(x => x.ClientId.HasValue);

        RuleFor(x => x.ClientLocationId)
            .GreaterThan(0).WithMessage("Client location ID must be greater than 0")
            .When(x => x.ClientLocationId.HasValue);

        RuleFor(x => x.TechnicianId)
            .GreaterThan(0).WithMessage("Technician user ID must be greater than 0")
            .When(x => x.TechnicianId.HasValue);

        RuleFor(x => x.SupervisorId)
            .GreaterThan(0).WithMessage("Supervisor user ID must be greater than 0")
            .When(x => x.SupervisorId.HasValue);

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

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
