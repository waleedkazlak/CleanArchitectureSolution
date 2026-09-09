using MediatR;

namespace CleanSample.Application.Commands.Issue;

public class DeleteIssueCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteIssueCommand(long id)
    {
        Id = id;
    }
}
