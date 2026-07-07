using CleanSample.Application.Queries.Product;
using FluentValidation;

namespace CleanSample.Application.Validators;

/// <summary>
/// Validator for GetProductByIdQuery
/// </summary>
public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product id is required")
            .GreaterThan(0)
            .WithMessage("Product id must be greater than 0");
    }
}