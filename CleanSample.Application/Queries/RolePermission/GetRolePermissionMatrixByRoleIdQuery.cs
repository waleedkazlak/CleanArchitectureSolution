using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.RolePermission;

public class GetRolePermissionMatrixByRoleIdQuery : IRequest<RolePermissionMatrixDto>
{
    public int RoleId { get; set; }

    public GetRolePermissionMatrixByRoleIdQuery(int roleId)
    {
        RoleId = roleId;
    }
}
