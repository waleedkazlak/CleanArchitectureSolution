using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssuesByFieldJobIdQuery : IRequest<List<IssueDto>>
{
    public long FieldJobId { get; set; }

    public GetIssuesByFieldJobIdQuery(long fieldJobId)
    {
        FieldJobId = fieldJobId;
    }
}
