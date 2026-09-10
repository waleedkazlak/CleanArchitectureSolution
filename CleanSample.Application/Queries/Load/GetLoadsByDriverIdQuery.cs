using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Load;

public class GetLoadsByDriverIdQuery : IRequest<IEnumerable<LoadDto>>
{
    public int DriverId { get; set; }

    public GetLoadsByDriverIdQuery(int driverId)
    {
        DriverId = driverId;
    }
}
