using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Part;

public class GetPartByIdQuery : IRequest<PartDto?>
{
    public int Id { get; set; }

    public GetPartByIdQuery(int id)
    {
        Id = id;
    }
}
