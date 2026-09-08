using CleanSample.Application.Queries.Client;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetClientByIdQueryValidator : AbstractValidator<GetClientByIdQuery>
{
    public GetClientByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid client ID.");
    }
}
