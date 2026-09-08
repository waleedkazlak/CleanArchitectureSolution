using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class VehicleSearchFilterDtoValidator : AbstractValidator<VehicleSearchFilterDto>
{
    private readonly string[] _validSortFields = { "VehicleNumber", "PlateNumber", "CapacityKg", "CreatedAt" };
    private readonly string[] _validSortDirections = { "asc", "desc" };

    public VehicleSearchFilterDtoValidator()
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

        RuleFor(x => x.MinCapacityKg)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum capacity cannot be negative")
            .When(x => x.MinCapacityKg.HasValue);

        RuleFor(x => x.MaxCapacityKg)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum capacity cannot be negative")
            .When(x => x.MaxCapacityKg.HasValue);

        RuleFor(x => x)
            .Must(x => x.MinCapacityKg <= x.MaxCapacityKg)
            .WithMessage("Minimum capacity cannot be greater than maximum capacity")
            .When(x => x.MinCapacityKg.HasValue && x.MaxCapacityKg.HasValue);

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
