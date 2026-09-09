using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.User;

public class GetUsersByRoleIdQuery : IRequest<List<UserDto>>
{
    public int RoleId { get; set; }

    public GetUsersByRoleIdQuery()
    {
    }

    public GetUsersByRoleIdQuery(int roleId)
    {
        RoleId = roleId;
    }
}
