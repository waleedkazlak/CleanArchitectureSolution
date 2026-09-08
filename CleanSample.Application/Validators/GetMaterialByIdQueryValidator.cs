using CleanSample.Application.Queries.Material;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetMaterialByIdQueryValidator : AbstractValidator<GetMaterialByIdQuery>
{
    public GetMaterialByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid material ID.");
    }
}
