using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleLoad;

public class GetVehicleLoadsByVehicleIdQuery : IRequest<IEnumerable<VehicleLoadDto>>
{
    public int VehicleId { get; set; }

    public GetVehicleLoadsByVehicleIdQuery(int vehicleId)
    {
        VehicleId = vehicleId;
    }
}
