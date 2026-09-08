using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Pick;

public class GetPickByIdQuery : IRequest<PickDto?>
{
    public long Id { get; set; }

    public GetPickByIdQuery(long id)
    {
        Id = id;
    }
}
