using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.OrderLine;

public class GetOrderLinesByOrderIdQuery : IRequest<IEnumerable<OrderLineDto>>
{
    public long OrderId { get; set; }

    public GetOrderLinesByOrderIdQuery(long orderId)
    {
        OrderId = orderId;
    }
}
