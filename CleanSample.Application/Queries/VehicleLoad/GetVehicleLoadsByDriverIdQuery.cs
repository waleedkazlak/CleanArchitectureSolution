using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleLoad;

public class GetVehicleLoadsByDriverIdQuery : IRequest<IEnumerable<VehicleLoadDto>>
{
    public int DriverId { get; set; }

    public GetVehicleLoadsByDriverIdQuery(int driverId)
    {
        DriverId = driverId;
    }
}
