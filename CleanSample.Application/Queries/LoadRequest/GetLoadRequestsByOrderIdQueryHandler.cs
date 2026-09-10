using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequest;

public class GetLoadRequestsByOrderIdQueryHandler : IRequestHandler<GetLoadRequestsByOrderIdQuery, IEnumerable<LoadRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLoadRequestsByOrderIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<LoadRequestDto>> Handle(GetLoadRequestsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var requests = await _unitOfWork.LoadRequests.GetByOrderIdAsync(request.OrderId);

        return requests.Select(lr => new LoadRequestDto
        {
            Id = lr.Id,
            RequestNumber = lr.RequestNumber,
            OrderId = lr.OrderId,
            OrderNumber = lr.Order?.OrderNumber,
            ClientId = lr.ClientId,
            ClientName = lr.Client?.Name,
            ClientLocationId = lr.ClientLocationId,
            ClientLocationName = lr.ClientLocation?.Name,
            RequestedBy = lr.RequestedBy,
            RequesterName = lr.Requester?.FullName,
            RequestDate = lr.RequestDate,
            ExecutionDate = lr.ExecutionDate,
            Status = lr.Status,
            DestinationAddress = lr.DestinationAddress,
            DestinationCity = lr.DestinationCity,
            Description = lr.Description,
            DriverId = lr.DriverId,
            DriverName = lr.Driver?.FullName,
            VehicleId = lr.VehicleId,
            VehicleNumber = lr.Vehicle?.VehicleNumber,
            PlateNumber = lr.Vehicle?.PlateNumber,
            Verified = lr.Verified,
            CreatedAt = lr.CreatedAt,
            UpdatedAt = lr.UpdatedAt
        });
    }
}
