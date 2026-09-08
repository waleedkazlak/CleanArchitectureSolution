using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Design;

public class GetDesignsWithFilterQuery : IRequest<PaginatedResultDto<DesignDto>>
{
    public DesignSearchFilterDto Filter { get; set; } = new();

    public GetDesignsWithFilterQuery()
    {
    }

    public GetDesignsWithFilterQuery(DesignSearchFilterDto filter)
    {
        Filter = filter ?? new DesignSearchFilterDto();
    }
}
