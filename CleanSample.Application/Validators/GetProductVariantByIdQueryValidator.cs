using CleanSample.Application.Queries.ProductVariant;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetProductVariantByIdQueryValidator : AbstractValidator<GetProductVariantByIdQuery>
{
    public GetProductVariantByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid variant ID.");
    }
}
