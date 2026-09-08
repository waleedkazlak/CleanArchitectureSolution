using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class ProductVariantSearchFilterDtoValidator : AbstractValidator<ProductVariantSearchFilterDto>
{
    private readonly string[] _validSortFields = { "Code", "Barcode", "ProductId", "CreatedAt" };
    private readonly string[] _validSortDirections = { "asc", "desc" };

    public ProductVariantSearchFilterDtoValidator()
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

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0")
            .When(x => x.ProductId.HasValue);

        RuleFor(x => x.ColorId)
            .GreaterThan(0).WithMessage("Color ID must be greater than 0")
            .When(x => x.ColorId.HasValue);

        RuleFor(x => x.MaterialId)
            .GreaterThan(0).WithMessage("Material ID must be greater than 0")
            .When(x => x.MaterialId.HasValue);

        RuleFor(x => x.DesignId)
            .GreaterThan(0).WithMessage("Design ID must be greater than 0")
            .When(x => x.DesignId.HasValue);

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
