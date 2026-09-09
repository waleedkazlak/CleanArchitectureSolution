using CleanSample.Application.DTOs;
using CleanSample.Application.Services;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.RolePermission;

public class SetRoleScreenPermissionCommandHandler : IRequestHandler<SetRoleScreenPermissionCommand, RolePermissionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionCheckerService _permissionCheckerService;
    private readonly ILogger<SetRoleScreenPermissionCommandHandler> _logger;

    public SetRoleScreenPermissionCommandHandler(
        IUnitOfWork unitOfWork,
        IPermissionCheckerService permissionCheckerService,
        ILogger<SetRoleScreenPermissionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _permissionCheckerService = permissionCheckerService;
        _logger = logger;
    }

    public async Task<RolePermissionDto> Handle(SetRoleScreenPermissionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Setting permission for RoleId: {RoleId}, ScreenId: {ScreenId}", request.RoleId, request.ScreenId);

        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);

        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {request.RoleId} not found.");
        }

        var screen = await _unitOfWork.Screens.GetByIdAsync(request.ScreenId, cancellationToken);
        if (screen == null)
        {
            throw new KeyNotFoundException($"Screen with ID {request.ScreenId} not found.");
        }

        var existing = await _unitOfWork.RolePermissions.GetByRoleAndScreenIdAsync(request.RoleId, request.ScreenId, cancellationToken);
        if (existing != null)
        {
            existing.CanView = request.CanView;
            existing.CanCreate = request.CanCreate;
            existing.CanUpdate = request.CanUpdate;
            existing.CanDelete = request.CanDelete;
            existing.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.RolePermissions.UpdateAsync(existing, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _permissionCheckerService.InvalidateRoleCache(request.RoleId);

            return new RolePermissionDto
            {
                RolePermissionId = existing.Id,
                RoleId = existing.RoleId,
                RoleName = role.Name,
                ScreenId = existing.ScreenId,
                ScreenName = screen.Name,
                ScreenCode = screen.Code,
                Module = screen.Module,
                CanView = existing.CanView,
                CanCreate = existing.CanCreate,
                CanUpdate = existing.CanUpdate,
                CanDelete = existing.CanDelete,
                CreatedAt = existing.CreatedAt,
                UpdatedAt = existing.UpdatedAt
            };
        }

        var permission = new Domain.Entities.RolePermission
        {
            RoleId = request.RoleId,
            ScreenId = request.ScreenId,
            CanView = request.CanView,
            CanCreate = request.CanCreate,
            CanUpdate = request.CanUpdate,
            CanDelete = request.CanDelete,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RolePermissions.AddAsync(permission, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _permissionCheckerService.InvalidateRoleCache(request.RoleId);

        return new RolePermissionDto
        {
            RolePermissionId = permission.Id,
            RoleId = permission.RoleId,
            RoleName = role.Name,
            ScreenId = permission.ScreenId,
            ScreenName = screen.Name,
            ScreenCode = screen.Code,
            Module = screen.Module,
            CanView = permission.CanView,
            CanCreate = permission.CanCreate,
            CanUpdate = permission.CanUpdate,
            CanDelete = permission.CanDelete,
            CreatedAt = permission.CreatedAt,
            UpdatedAt = permission.UpdatedAt
        };
    }
}
