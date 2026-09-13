using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.User;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            RoleId = user.RoleId,
            RoleName = CleanSample.Application.Helpers.LocalizationHelper.Localize(user.Role?.NameEn, user.Role?.NameAr),
            UserName = user.UserName,
            FullName = CleanSample.Application.Helpers.LocalizationHelper.Localize(user.FullNameEn, user.FullNameAr) ?? user.FullNameEn,
            FullNameEn = user.FullNameEn,
            FullNameAr = user.FullNameAr,
            Email = user.Email,
            Mobile = user.Mobile,
            PreferredLanguage = user.PreferredLanguage,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
