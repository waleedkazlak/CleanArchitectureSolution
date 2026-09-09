using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobByIdQuery : IRequest<FieldJobDto?>
{
    public long Id { get; set; }

    public GetFieldJobByIdQuery(long id)
    {
        Id = id;
    }
}
