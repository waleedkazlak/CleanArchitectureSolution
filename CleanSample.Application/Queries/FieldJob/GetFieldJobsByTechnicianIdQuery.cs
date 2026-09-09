using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobsByTechnicianIdQuery : IRequest<List<FieldJobDto>>
{
    public int TechnicianId { get; set; }

    public GetFieldJobsByTechnicianIdQuery(int technicianId)
    {
        TechnicianId = technicianId;
    }
}
