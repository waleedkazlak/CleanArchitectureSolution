using CleanSample.Application.Queries.ProductBOM;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetProductBOMByIdQueryValidator : AbstractValidator<GetProductBOMByIdQuery>
{
    public GetProductBOMByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid Product BOM ID.");
    }
}
