using CleanSample.Application.Services;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.RolePermission;

public class BatchUpdateRolePermissionsCommandHandler : IRequestHandler<BatchUpdateRolePermissionsCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionCheckerService _permissionCheckerService;
    private readonly ILogger<BatchUpdateRolePermissionsCommandHandler> _logger;

    public BatchUpdateRolePermissionsCommandHandler(
        IUnitOfWork unitOfWork,
        IPermissionCheckerService permissionCheckerService,
        ILogger<BatchUpdateRolePermissionsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _permissionCheckerService = permissionCheckerService;
        _logger = logger;
    }

    public async Task<bool> Handle(BatchUpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var roleId = request.Request.RoleId;
        _logger.LogInformation("Batch updating permissions for RoleId: {RoleId}", roleId);

        var role = await _unitOfWork.Roles.GetByIdAsync(roleId);

        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {roleId} not found.");
        }

        var entities = request.Request.Permissions.Select(p => new Domain.Entities.RolePermission
        {
            RoleId = roleId,
            ScreenId = p.ScreenId,
            CanView = p.CanView,
            CanCreate = p.CanCreate,
            CanUpdate = p.CanUpdate,
            CanDelete = p.CanDelete
        }).ToList();

        await _unitOfWork.RolePermissions.SetPermissionsBatchAsync(roleId, entities, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _permissionCheckerService.InvalidateRoleCache(roleId);

        _logger.LogInformation("Permissions batch updated successfully for RoleId: {RoleId}", roleId);
        return true;
    }
}
