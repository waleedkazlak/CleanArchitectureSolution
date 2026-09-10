using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Load;

public class GetLoadsWithFilterQuery : IRequest<PaginatedResultDto<LoadDto>>
{
    public LoadSearchFilterDto Filter { get; set; }

    public GetLoadsWithFilterQuery(LoadSearchFilterDto filter)
    {
        Filter = filter;
    }
}
