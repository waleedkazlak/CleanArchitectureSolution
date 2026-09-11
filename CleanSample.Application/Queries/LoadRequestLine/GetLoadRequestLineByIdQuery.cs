using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequestLine;

public class GetLoadRequestLineByIdQuery : IRequest<LoadRequestLineDto?>
{
    public long Id { get; set; }

    public GetLoadRequestLineByIdQuery(long id)
    {
        Id = id;
    }
}
