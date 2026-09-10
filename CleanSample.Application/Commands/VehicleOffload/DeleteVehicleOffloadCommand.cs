using MediatR;

namespace CleanSample.Application.Commands.VehicleOffload;

public class DeleteVehicleOffloadCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteVehicleOffloadCommand(long id)
    {
        Id = id;
    }
}
