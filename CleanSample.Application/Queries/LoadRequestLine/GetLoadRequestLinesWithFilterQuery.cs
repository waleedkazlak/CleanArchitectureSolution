using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequestLine;

public class GetLoadRequestLinesWithFilterQuery : IRequest<PaginatedResultDto<LoadRequestLineDto>>
{
    public LoadRequestLineSearchFilterDto Filter { get; set; }

    public GetLoadRequestLinesWithFilterQuery(LoadRequestLineSearchFilterDto filter)
    {
        Filter = filter;
    }
}
