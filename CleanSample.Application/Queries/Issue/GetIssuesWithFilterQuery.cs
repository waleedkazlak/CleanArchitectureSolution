using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssuesWithFilterQuery : IRequest<PaginatedResultDto<IssueDto>>
{
    public IssueSearchFilterDto Filter { get; set; }

    public GetIssuesWithFilterQuery(IssueSearchFilterDto filter)
    {
        Filter = filter;
    }
}
