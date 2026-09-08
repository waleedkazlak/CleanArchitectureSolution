using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Pick;

public class GetPicksByDriverIdQuery : IRequest<IEnumerable<PickDto>>
{
    public int DriverId { get; set; }

    public GetPicksByDriverIdQuery(int driverId)
    {
        DriverId = driverId;
    }
}
