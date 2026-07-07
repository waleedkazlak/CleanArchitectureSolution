using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

/// <summary>
/// Validator for ProductSearchFilterDto
/// </summary>
public class ProductSearchFilterDtoValidator : AbstractValidator<ProductSearchFilterDto>
{
    private readonly string[] _validSortFields = { "Name", "Price", "Stock", "CreatedAt" };
    private readonly string[] _validSortDirections = { "asc", "desc" };

    public ProductSearchFilterDtoValidator()
    {
        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .WithMessage("Page number is required")
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .NotEmpty()
            .WithMessage("Page size is required")
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(255)
            .WithMessage("Search term cannot exceed 255 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum price cannot be negative")
            .When(x => x.MinPrice.HasValue);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maximum price cannot be negative")
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x)
            .Must(x => x.MinPrice <= x.MaxPrice)
            .WithMessage("Minimum price cannot be greater than maximum price")
            .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock cannot be negative")
            .When(x => x.MinStock.HasValue);

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