using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.PickRequest;

public class GetPickRequestsWithFilterQuery : IRequest<PaginatedResultDto<PickRequestDto>>
{
    public PickRequestSearchFilterDto Filter { get; set; }

    public GetPickRequestsWithFilterQuery(PickRequestSearchFilterDto filter)
    {
        Filter = filter;
    }
}
