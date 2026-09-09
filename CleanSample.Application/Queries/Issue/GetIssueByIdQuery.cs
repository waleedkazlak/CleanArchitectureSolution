using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssueByIdQuery : IRequest<IssueDto?>
{
    public long Id { get; set; }

    public GetIssueByIdQuery(long id)
    {
        Id = id;
    }
}
