using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobsBySupervisorIdQuery : IRequest<List<FieldJobDto>>
{
    public int SupervisorId { get; set; }

    public GetFieldJobsBySupervisorIdQuery(int supervisorId)
    {
        SupervisorId = supervisorId;
    }
}
