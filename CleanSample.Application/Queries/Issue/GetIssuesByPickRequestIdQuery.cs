using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssuesByPickRequestIdQuery : IRequest<List<IssueDto>>
{
    public long PickRequestId { get; set; }

    public GetIssuesByPickRequestIdQuery(long pickRequestId)
    {
        PickRequestId = pickRequestId;
    }
}
