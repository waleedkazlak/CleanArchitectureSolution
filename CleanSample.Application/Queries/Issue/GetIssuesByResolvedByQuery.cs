using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssuesByResolvedByQuery : IRequest<List<IssueDto>>
{
    public int ResolvedBy { get; set; }

    public GetIssuesByResolvedByQuery(int resolvedBy)
    {
        ResolvedBy = resolvedBy;
    }
}
