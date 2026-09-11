using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleLoad;

public class GetVehicleLoadsByLoadRequestIdQuery : IRequest<IEnumerable<VehicleLoadDto>>
{
    public long LoadRequestId { get; set; }

    public GetVehicleLoadsByLoadRequestIdQuery(long loadRequestId)
    {
        LoadRequestId = loadRequestId;
    }
}
