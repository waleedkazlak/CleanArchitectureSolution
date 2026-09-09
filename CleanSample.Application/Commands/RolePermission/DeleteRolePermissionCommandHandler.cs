using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.RolePermission;

public class DeleteRolePermissionCommandHandler : IRequestHandler<DeleteRolePermissionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteRolePermissionCommandHandler> _logger;

    public DeleteRolePermissionCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteRolePermissionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteRolePermissionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting RolePermission ID: {RolePermissionId}", request.Id);

        var permission = await _unitOfWork.RolePermissions.GetByIdAsync(request.Id, cancellationToken);
        if (permission == null)
        {
            throw new KeyNotFoundException($"RolePermission with ID {request.Id} not found.");
        }

        await _unitOfWork.RolePermissions.DeleteAsync(permission, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("RolePermission ID: {RolePermissionId} deleted successfully", request.Id);
        return true;
    }
}
