using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.RolePermission;

public class BatchUpdateRolePermissionsCommand : IRequest<bool>
{
    public BatchUpdateRolePermissionsDto Request { get; set; }

    public BatchUpdateRolePermissionsCommand(BatchUpdateRolePermissionsDto request)
    {
        Request = request;
    }
}
