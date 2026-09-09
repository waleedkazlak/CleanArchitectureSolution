using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Role;

public class GetRoleByIdQuery : IRequest<RoleDto?>
{
    public int Id { get; set; }
}
