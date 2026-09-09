using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.RolePermission;

public class SetRoleScreenPermissionCommand : IRequest<RolePermissionDto>
{
    public int RoleId { get; set; }
    public int ScreenId { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
}
