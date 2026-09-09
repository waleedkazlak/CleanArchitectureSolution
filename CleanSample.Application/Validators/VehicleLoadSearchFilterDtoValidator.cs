using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class VehicleLoadSearchFilterDtoValidator : AbstractValidator<VehicleLoadSearchFilterDto>
{
    private readonly string[] _validSortFields = { "LoadDate", "PickRequestId", "VehicleId", "DriverId", "Status", "CreatedAt" };
    private readonly string[] _validSortDirections = { "asc", "desc" };

    public VehicleLoadSearchFilterDtoValidator()
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

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0")
            .When(x => x.VehicleId.HasValue);

        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Driver ID must be greater than 0")
            .When(x => x.DriverId.HasValue);

        RuleFor(x => x.VerifiedBy)
            .GreaterThan(0).WithMessage("VerifiedBy user ID must be greater than 0")
            .When(x => x.VerifiedBy.HasValue);

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
