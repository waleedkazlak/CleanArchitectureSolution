using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequestLine;

public class GetLoadRequestLinesByLoadRequestIdQuery : IRequest<IEnumerable<LoadRequestLineDto>>
{
    public long LoadRequestId { get; set; }

    public GetLoadRequestLinesByLoadRequestIdQuery(long loadRequestId)
    {
        LoadRequestId = loadRequestId;
    }
}
