using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Order;

public class GetOrdersByClientIdQuery : IRequest<IEnumerable<OrderDto>>
{
    public int ClientId { get; set; }

    public GetOrdersByClientIdQuery(int clientId)
    {
        ClientId = clientId;
    }
}
