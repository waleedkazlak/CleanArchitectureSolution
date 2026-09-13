using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.User;

public class GetUsersByRoleIdQueryHandler : IRequestHandler<GetUsersByRoleIdQuery, List<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUsersByRoleIdQueryHandler> _logger;

    public GetUsersByRoleIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUsersByRoleIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<UserDto>> Handle(GetUsersByRoleIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetUsersByRoleIdQuery for RoleId: {RoleId}", request.RoleId);

        var users = await _unitOfWork.Users.GetByRoleIdAsync(request.RoleId);

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            RoleId = u.RoleId,
            RoleName = CleanSample.Application.Helpers.LocalizationHelper.Localize(u.Role?.NameEn, u.Role?.NameAr),
            UserName = u.UserName,
            FullName = CleanSample.Application.Helpers.LocalizationHelper.Localize(u.FullNameEn, u.FullNameAr) ?? u.FullNameEn,
            FullNameEn = u.FullNameEn,
            FullNameAr = u.FullNameAr,
            Email = u.Email,
            Mobile = u.Mobile,
            PreferredLanguage = u.PreferredLanguage,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt
        }).ToList();
    }
}
