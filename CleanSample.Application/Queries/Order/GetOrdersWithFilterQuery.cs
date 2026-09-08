using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Order;

public class GetOrdersWithFilterQuery : IRequest<PaginatedResultDto<OrderDto>>
{
    public OrderSearchFilterDto Filter { get; set; }

    public GetOrdersWithFilterQuery(OrderSearchFilterDto filter)
    {
        Filter = filter;
    }
}
