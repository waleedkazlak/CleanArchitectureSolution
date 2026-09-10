using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Load;

public class GetLoadsByLoadRequestIdQuery : IRequest<IEnumerable<LoadDto>>
{
    public long LoadRequestId { get; set; }

    public GetLoadsByLoadRequestIdQuery(long loadRequestId)
    {
        LoadRequestId = loadRequestId;
    }
}
