using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.VehicleOffload;

public class CreateVehicleOffloadsCommand : IRequest<List<VehicleOffloadDto>>
{
    public List<CreateVehicleOffloadItemDto> Offloads { get; set; } = new();

    public CreateVehicleOffloadsCommand()
    {
    }

    public CreateVehicleOffloadsCommand(List<CreateVehicleOffloadItemDto> offloads)
    {
        Offloads = offloads;
    }
}
