using MediatR;

namespace CleanSample.Application.Commands.VehicleLoad;

public class DeleteVehicleLoadsCommand : IRequest<bool>
{
    public List<long> LoadIds { get; set; } = new();

    public DeleteVehicleLoadsCommand()
    {
    }

    public DeleteVehicleLoadsCommand(List<long> loadIds)
    {
        LoadIds = loadIds;
    }

    public DeleteVehicleLoadsCommand(long loadId)
    {
        LoadIds = new List<long> { loadId };
    }
}
