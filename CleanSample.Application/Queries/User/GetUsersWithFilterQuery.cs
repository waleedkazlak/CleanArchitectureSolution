using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.User;

public class GetUsersWithFilterQuery : IRequest<PaginatedResultDto<UserDto>>
{
    public UserSearchFilterDto Filter { get; set; } = new();

    public GetUsersWithFilterQuery()
    {
    }

    public GetUsersWithFilterQuery(UserSearchFilterDto filter)
    {
        Filter = filter ?? new UserSearchFilterDto();
    }
}
