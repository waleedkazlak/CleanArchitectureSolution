using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Pick;

public class GetPicksWithFilterQuery : IRequest<PaginatedResultDto<PickDto>>
{
    public PickSearchFilterDto Filter { get; set; }

    public GetPicksWithFilterQuery(PickSearchFilterDto filter)
    {
        Filter = filter;
    }
}
