using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class LoadRequestSearchFilterDtoValidator : AbstractValidator<LoadRequestSearchFilterDto>
{
    private readonly string[] _validSortFields = { "ClientId", "OrderId", "RequestDate", "ExecutionDate", "Status", "CreatedAt" };
    private readonly string[] _validSortDirections = { "asc", "desc" };

    public LoadRequestSearchFilterDtoValidator()
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

        RuleFor(x => x.OrderId)
            .GreaterThan(0).WithMessage("Order ID must be greater than 0")
            .When(x => x.OrderId.HasValue);

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Client ID must be greater than 0")
            .When(x => x.ClientId.HasValue);

        RuleFor(x => x.ClientLocationId)
            .GreaterThan(0).WithMessage("Client location ID must be greater than 0")
            .When(x => x.ClientLocationId.HasValue);

        RuleFor(x => x.RequestedBy)
            .GreaterThan(0).WithMessage("RequestedBy user ID must be greater than 0")
            .When(x => x.RequestedBy.HasValue);

        RuleFor(x => x.DriverId)
            .GreaterThan(0).WithMessage("Driver ID must be greater than 0")
            .When(x => x.DriverId.HasValue);

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0")
            .When(x => x.VehicleId.HasValue);

        RuleFor(x => x.Status)
            .Must(s => !s.HasValue || Enum.IsDefined(typeof(Domain.Enums.LoadRequestStatusEnum), s.Value))
            .WithMessage("Status must be a valid LoadRequestStatus value.");

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
