using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleOffload;

public class GetVehicleOffloadByIdQuery : IRequest<VehicleOffloadDto?>
{
    public long Id { get; set; }

    public GetVehicleOffloadByIdQuery(long id)
    {
        Id = id;
    }
}
