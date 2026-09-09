using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.RolePermission;

public class GetRolePermissionMatrixByRoleIdQueryHandler : IRequestHandler<GetRolePermissionMatrixByRoleIdQuery, RolePermissionMatrixDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetRolePermissionMatrixByRoleIdQueryHandler> _logger;

    public GetRolePermissionMatrixByRoleIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetRolePermissionMatrixByRoleIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<RolePermissionMatrixDto> Handle(GetRolePermissionMatrixByRoleIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting permission matrix for RoleId: {RoleId}", request.RoleId);

        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);

        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {request.RoleId} not found.");
        }

        var screens = await _unitOfWork.Screens.GetActiveAsync(cancellationToken);
        var existingPermissions = await _unitOfWork.RolePermissions.GetByRoleIdAsync(request.RoleId, cancellationToken);

        var permissionsDict = existingPermissions.ToDictionary(p => p.ScreenId);

        var items = screens.Select(screen =>
        {
            if (permissionsDict.TryGetValue(screen.Id, out var perm))
            {
                return new ScreenPermissionItemDto
                {
                    ScreenId = screen.Id,
                    ScreenName = screen.Name,
                    ScreenCode = screen.Code,
                    Module = screen.Module,
                    Description = screen.Description,
                    CanView = perm.CanView,
                    CanCreate = perm.CanCreate,
                    CanUpdate = perm.CanUpdate,
                    CanDelete = perm.CanDelete
                };
            }

            return new ScreenPermissionItemDto
            {
                ScreenId = screen.Id,
                ScreenName = screen.Name,
                ScreenCode = screen.Code,
                Module = screen.Module,
                Description = screen.Description,
                CanView = false,
                CanCreate = false,
                CanUpdate = false,
                CanDelete = false
            };
        }).ToList();

        return new RolePermissionMatrixDto
        {
            RoleId = role.Id,
            RoleName = role.Name,
            Permissions = items
        };
    }
}
