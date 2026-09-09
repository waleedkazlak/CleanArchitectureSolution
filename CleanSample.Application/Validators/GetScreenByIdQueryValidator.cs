using CleanSample.Application.Queries.Screen;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetScreenByIdQueryValidator : AbstractValidator<GetScreenByIdQuery>
{
    public GetScreenByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Valid Screen ID is required");
    }
}
