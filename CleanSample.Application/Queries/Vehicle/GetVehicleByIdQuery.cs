using MediatR;
using CleanSample.Application.DTOs;

namespace CleanSample.Application.Queries.Vehicle;

public class GetVehicleByIdQuery : IRequest<VehicleDto?>
{
    public int Id { get; set; }
}
