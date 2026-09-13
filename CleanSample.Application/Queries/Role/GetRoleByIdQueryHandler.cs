using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Role;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRoleByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(request.Id);
        if (role == null)
        {
            return null;
        }

        return new RoleDto
        {
            Id = role.Id,
            Name = CleanSample.Application.Helpers.LocalizationHelper.Localize(role.NameEn, role.NameAr) ?? role.NameEn,
            NameEn = role.NameEn,
            NameAr = role.NameAr,
            Description = CleanSample.Application.Helpers.LocalizationHelper.Localize(role.DescriptionEn, role.DescriptionAr),
            DescriptionEn = role.DescriptionEn,
            DescriptionAr = role.DescriptionAr,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt
        };
    }
}
