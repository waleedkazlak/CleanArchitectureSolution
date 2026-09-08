using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Vehicle;

public class GetVehiclesWithFilterQuery : IRequest<PaginatedResultDto<VehicleDto>>
{
    public VehicleSearchFilterDto Filter { get; set; } = new();

    public GetVehiclesWithFilterQuery()
    {
    }

    public GetVehiclesWithFilterQuery(VehicleSearchFilterDto filter)
    {
        Filter = filter ?? new VehicleSearchFilterDto();
    }
}
