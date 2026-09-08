using MediatR;

namespace CleanSample.Application.Commands.Vehicle;

public class DeleteVehicleCommand : IRequest<bool>
{
    public int Id { get; set; }
}
