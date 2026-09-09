using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobsByPickRequestIdQuery : IRequest<List<FieldJobDto>>
{
    public long PickRequestId { get; set; }

    public GetFieldJobsByPickRequestIdQuery(long pickRequestId)
    {
        PickRequestId = pickRequestId;
    }
}
