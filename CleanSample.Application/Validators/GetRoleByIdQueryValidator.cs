using CleanSample.Application.Queries.Role;
using FluentValidation;

namespace CleanSample.Application.Validators;

public class GetRoleByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid role ID.");
    }
}
