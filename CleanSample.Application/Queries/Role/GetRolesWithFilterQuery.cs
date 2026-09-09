using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Role;

public class GetRolesWithFilterQuery : IRequest<PaginatedResultDto<RoleDto>>
{
    public RoleSearchFilterDto Filter { get; set; } = new();

    public GetRolesWithFilterQuery()
    {
    }

    public GetRolesWithFilterQuery(RoleSearchFilterDto filter)
    {
        Filter = filter ?? new RoleSearchFilterDto();
    }
}
