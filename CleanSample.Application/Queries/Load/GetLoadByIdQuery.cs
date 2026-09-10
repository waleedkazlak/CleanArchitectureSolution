using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Load;

public class GetLoadByIdQuery : IRequest<LoadDto?>
{
    public long Id { get; set; }

    public GetLoadByIdQuery(long id)
    {
        Id = id;
    }
}
