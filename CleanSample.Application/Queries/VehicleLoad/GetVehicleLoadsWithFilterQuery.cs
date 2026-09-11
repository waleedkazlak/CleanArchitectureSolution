using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleLoad;

public class GetVehicleLoadsWithFilterQuery : IRequest<PaginatedResultDto<VehicleLoadDto>>
{
    public VehicleLoadSearchFilterDto Filter { get; set; }

    public GetVehicleLoadsWithFilterQuery(VehicleLoadSearchFilterDto filter)
    {
        Filter = filter;
    }
}
