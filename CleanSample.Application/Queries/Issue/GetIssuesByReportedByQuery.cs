using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssuesByReportedByQuery : IRequest<List<IssueDto>>
{
    public int ReportedBy { get; set; }

    public GetIssuesByReportedByQuery(int reportedBy)
    {
        ReportedBy = reportedBy;
    }
}
