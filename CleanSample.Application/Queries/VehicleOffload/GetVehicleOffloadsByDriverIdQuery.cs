using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleOffload;

public class GetVehicleOffloadsByDriverIdQuery : IRequest<IEnumerable<VehicleOffloadDto>>
{
    public int DriverId { get; set; }

    public GetVehicleOffloadsByDriverIdQuery(int driverId)
    {
        DriverId = driverId;
    }
}
