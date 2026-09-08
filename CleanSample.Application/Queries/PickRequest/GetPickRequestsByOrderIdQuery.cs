using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.PickRequest;

public class GetPickRequestsByOrderIdQuery : IRequest<IEnumerable<PickRequestDto>>
{
    public long OrderId { get; set; }

    public GetPickRequestsByOrderIdQuery(long orderId)
    {
        OrderId = orderId;
    }
}
