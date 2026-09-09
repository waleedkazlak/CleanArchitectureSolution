using MediatR;

namespace CleanSample.Application.Commands.RolePermission;

public class DeleteRolePermissionCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteRolePermissionCommand(int id)
    {
        Id = id;
    }
}
