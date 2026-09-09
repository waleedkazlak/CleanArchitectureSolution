using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesBySupervisorIdQuery : IRequest<List<FieldAssemblyDto>>
{
    public int SupervisorId { get; set; }

    public GetFieldAssembliesBySupervisorIdQuery(int supervisorId)
    {
        SupervisorId = supervisorId;
    }
}
