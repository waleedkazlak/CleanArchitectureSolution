using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.ClientLocation;

public class GetClientLocationsWithFilterQuery : IRequest<PaginatedResultDto<ClientLocationDto>>
{
    public ClientLocationSearchFilterDto Filter { get; set; }

    public GetClientLocationsWithFilterQuery(ClientLocationSearchFilterDto filter)
    {
        Filter = filter;
    }
}
