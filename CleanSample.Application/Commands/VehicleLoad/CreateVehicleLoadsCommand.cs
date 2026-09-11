using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.VehicleLoad;

public class CreateVehicleLoadsCommand : IRequest<List<VehicleLoadDto>>
{
    public List<CreateVehicleLoadItemDto> Loads { get; set; } = new();

    public CreateVehicleLoadsCommand()
    {
    }

    public CreateVehicleLoadsCommand(List<CreateVehicleLoadItemDto> loads)
    {
        Loads = loads;
    }
}
