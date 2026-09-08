using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Order;

public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public long Id { get; set; }

    public GetOrderByIdQuery(long id)
    {
        Id = id;
    }
}
