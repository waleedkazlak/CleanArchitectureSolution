using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.OrderLine;

public class GetOrderLineByIdQuery : IRequest<OrderLineDto?>
{
    public long Id { get; set; }

    public GetOrderLineByIdQuery(long id)
    {
        Id = id;
    }
}
