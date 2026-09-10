using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequest;

public class GetLoadRequestsByOrderIdQuery : IRequest<IEnumerable<LoadRequestDto>>
{
    public long OrderId { get; set; }

    public GetLoadRequestsByOrderIdQuery(long orderId)
    {
        OrderId = orderId;
    }
}
