using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.RolePermission;

public class CheckPermissionQueryHandler : IRequestHandler<CheckPermissionQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CheckPermissionQueryHandler> _logger;

    public CheckPermissionQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<CheckPermissionQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(CheckPermissionQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking permission for RoleId: {RoleId}, ScreenCode: {ScreenCode}, Action: {Action}",
            request.RoleId, request.ScreenCode, request.Action);

        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);

        if (role == null) return false;

        // Admin has full access to everything
        if (role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var permission = await _unitOfWork.RolePermissions.GetByRoleAndScreenCodeAsync(
            request.RoleId, request.ScreenCode.ToUpperInvariant(), cancellationToken);

        if (permission == null) return false;

        return request.Action.ToLowerInvariant() switch
        {
            "view" or "read" or "get" => permission.CanView,
            "create" or "add" or "post" => permission.CanCreate,
            "update" or "edit" or "put" or "patch" => permission.CanUpdate,
            "delete" or "remove" => permission.CanDelete,
            _ => false
        };
    }
}
