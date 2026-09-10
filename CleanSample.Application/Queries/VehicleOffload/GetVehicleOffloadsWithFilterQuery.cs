using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleOffload;

public class GetVehicleOffloadsWithFilterQuery : IRequest<PaginatedResultDto<VehicleOffloadDto>>
{
    public VehicleOffloadSearchFilterDto Filter { get; set; }

    public GetVehicleOffloadsWithFilterQuery(VehicleOffloadSearchFilterDto filter)
    {
        Filter = filter;
    }
}
