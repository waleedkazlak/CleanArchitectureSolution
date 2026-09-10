using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssuesByLoadRequestIdQuery : IRequest<List<IssueDto>>
{
    public long LoadRequestId { get; set; }

    public GetIssuesByLoadRequestIdQuery(long loadRequestId)
    {
        LoadRequestId = loadRequestId;
    }
}
