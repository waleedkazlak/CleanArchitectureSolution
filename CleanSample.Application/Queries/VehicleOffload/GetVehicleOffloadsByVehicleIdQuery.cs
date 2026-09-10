using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleOffload;

public class GetVehicleOffloadsByVehicleIdQuery : IRequest<IEnumerable<VehicleOffloadDto>>
{
    public int VehicleId { get; set; }

    public GetVehicleOffloadsByVehicleIdQuery(int vehicleId)
    {
        VehicleId = vehicleId;
    }
}
