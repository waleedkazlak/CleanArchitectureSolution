using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesByFieldJobIdQuery : IRequest<List<FieldAssemblyDto>>
{
    public long FieldJobId { get; set; }

    public GetFieldAssembliesByFieldJobIdQuery(long fieldJobId)
    {
        FieldJobId = fieldJobId;
    }
}
