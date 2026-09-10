using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequest;

public class GetLoadRequestByIdQuery : IRequest<LoadRequestDto?>
{
    public long Id { get; set; }

    public GetLoadRequestByIdQuery(long id)
    {
        Id = id;
    }
}
