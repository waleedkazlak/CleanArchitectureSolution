using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.OrderLine;

public class GetOrderLinesWithFilterQuery : IRequest<PaginatedResultDto<OrderLineDto>>
{
    public OrderLineSearchFilterDto Filter { get; set; }

    public GetOrderLinesWithFilterQuery(OrderLineSearchFilterDto filter)
    {
        Filter = filter;
    }
}
