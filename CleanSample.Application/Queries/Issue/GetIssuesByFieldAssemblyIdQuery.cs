using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssuesByFieldAssemblyIdQuery : IRequest<List<IssueDto>>
{
    public long FieldAssemblyId { get; set; }

    public GetIssuesByFieldAssemblyIdQuery(long fieldAssemblyId)
    {
        FieldAssemblyId = fieldAssemblyId;
    }
}
