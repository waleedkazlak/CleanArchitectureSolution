using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequest;

public class GetLoadRequestsWithFilterQuery : IRequest<PaginatedResultDto<LoadRequestDto>>
{
    public LoadRequestSearchFilterDto Filter { get; set; }

    public GetLoadRequestsWithFilterQuery(LoadRequestSearchFilterDto filter)
    {
        Filter = filter;
    }
}
