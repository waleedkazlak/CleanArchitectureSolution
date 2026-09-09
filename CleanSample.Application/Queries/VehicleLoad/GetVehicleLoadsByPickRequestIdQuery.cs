using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleLoad;

public class GetVehicleLoadsByPickRequestIdQuery : IRequest<IEnumerable<VehicleLoadDto>>
{
    public long PickRequestId { get; set; }

    public GetVehicleLoadsByPickRequestIdQuery(long pickRequestId)
    {
        PickRequestId = pickRequestId;
    }
}
