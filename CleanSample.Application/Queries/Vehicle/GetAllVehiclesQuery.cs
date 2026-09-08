using MediatR;
using CleanSample.Application.DTOs;

namespace CleanSample.Application.Queries.Vehicle;

public class GetAllVehiclesQuery : IRequest<IEnumerable<VehicleDto>>
{
}
