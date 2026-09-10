using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.VehicleOffload;

public class GetVehicleOffloadsByLoadRequestIdQuery : IRequest<IEnumerable<VehicleOffloadDto>>
{
    public long LoadRequestId { get; set; }

    public GetVehicleOffloadsByLoadRequestIdQuery(long loadRequestId)
    {
        LoadRequestId = loadRequestId;
    }
}
