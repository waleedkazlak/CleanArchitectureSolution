using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Pick;

public class GetPicksByPickRequestIdQuery : IRequest<IEnumerable<PickDto>>
{
    public long PickRequestId { get; set; }

    public GetPicksByPickRequestIdQuery(long pickRequestId)
    {
        PickRequestId = pickRequestId;
    }
}
