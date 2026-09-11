using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleLoad;

public class GetVehicleLoadByIdQuery : IRequest<VehicleLoadDto?>
{
    public long Id { get; set; }

    public GetVehicleLoadByIdQuery(long id)
    {
        Id = id;
    }
}
