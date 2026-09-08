using MediatR;

namespace CleanSample.Application.Commands.Pick;

public class DeletePicksCommand : IRequest<bool>
{
    public List<long> PickIds { get; set; } = new();

    public DeletePicksCommand()
    {
    }

    public DeletePicksCommand(List<long> pickIds)
    {
        PickIds = pickIds;
    }

    public DeletePicksCommand(long pickId)
    {
        PickIds = new List<long> { pickId };
    }
}
