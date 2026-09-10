using MediatR;

namespace CleanSample.Application.Commands.Load;

public class DeleteLoadsCommand : IRequest<bool>
{
    public List<long> LoadIds { get; set; } = new();

    public DeleteLoadsCommand()
    {
    }

    public DeleteLoadsCommand(List<long> loadIds)
    {
        LoadIds = loadIds;
    }

    public DeleteLoadsCommand(long loadId)
    {
        LoadIds = new List<long> { loadId };
    }
}
