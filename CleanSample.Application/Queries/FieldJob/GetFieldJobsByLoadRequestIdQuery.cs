using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobsByLoadRequestIdQuery : IRequest<List<FieldJobDto>>
{
    public long LoadRequestId { get; set; }

    public GetFieldJobsByLoadRequestIdQuery(long loadRequestId)
    {
        LoadRequestId = loadRequestId;
    }
}
