using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssemblyByIdQuery : IRequest<FieldAssemblyDto?>
{
    public long Id { get; set; }

    public GetFieldAssemblyByIdQuery(long id)
    {
        Id = id;
    }
}
