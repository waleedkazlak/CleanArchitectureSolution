using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesByTechnicianIdQuery : IRequest<List<FieldAssemblyDto>>
{
    public int TechnicianId { get; set; }

    public GetFieldAssembliesByTechnicianIdQuery(int technicianId)
    {
        TechnicianId = technicianId;
    }
}
