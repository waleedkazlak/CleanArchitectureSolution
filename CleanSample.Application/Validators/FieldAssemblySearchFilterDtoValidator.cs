using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class FieldAssemblySearchFilterDtoValidator : AbstractValidator<FieldAssemblySearchFilterDto>
{
    private readonly string[] _validSortFields = { "FieldJobId", "ProductVariantId", "Quantity", "AssemblyDate", "Status", "CreatedAt" };
    private readonly string[] _validSortDirections = { "asc", "desc" };

    public FieldAssemblySearchFilterDtoValidator()
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

        RuleFor(x => x.FieldJobId)
            .GreaterThan(0).WithMessage("Field job ID must be greater than 0")
            .When(x => x.FieldJobId.HasValue);

        RuleFor(x => x.ProductVariantId)
            .GreaterThan(0).WithMessage("Product variant ID must be greater than 0")
            .When(x => x.ProductVariantId.HasValue);

        RuleFor(x => x.ProductBarcode)
            .MaximumLength(100).WithMessage("Product barcode cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ProductBarcode));

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
