using MediatR;

namespace CleanSample.Application.Commands.VehicleLoad;

public class DeleteVehicleLoadCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteVehicleLoadCommand(long id)
    {
        Id = id;
    }
}
