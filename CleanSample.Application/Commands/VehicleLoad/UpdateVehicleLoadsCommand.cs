using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.VehicleLoad;

public class UpdateVehicleLoadsCommand : IRequest<List<VehicleLoadDto>>
{
    public List<UpdateVehicleLoadItemDto> Loads { get; set; } = new();

    public UpdateVehicleLoadsCommand()
    {
    }

    public UpdateVehicleLoadsCommand(List<UpdateVehicleLoadItemDto> loads)
    {
        Loads = loads;
    }
}
