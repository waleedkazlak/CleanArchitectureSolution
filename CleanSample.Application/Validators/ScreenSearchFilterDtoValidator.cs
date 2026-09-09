using CleanSample.Application.DTOs;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class ScreenSearchFilterDtoValidator : AbstractValidator<ScreenSearchFilterDto>
{
    public ScreenSearchFilterDtoValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");

        RuleFor(x => x.SortDirection)
            .Must(x => string.IsNullOrEmpty(x) || x.ToLower() == "asc" || x.ToLower() == "desc")
            .WithMessage("Sort direction must be either 'asc' or 'desc'");
    }
}
