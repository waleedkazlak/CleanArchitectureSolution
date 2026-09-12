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
            OrderId = lr.OrderId,
            ClientId = lr.ClientId,
            ClientName = lr.Client?.Name,
            ClientLocationId = lr.ClientLocationId,
            ClientLocationName = lr.ClientLocation?.Name,
            RequestedBy = lr.RequestedBy,
            RequesterName = lr.Requester?.FullName,
            RequestDate = lr.RequestDate,
            ExecutionDate = lr.ExecutionDate,
            Status = lr.Status,
            StatusName = lr.LoadRequestStatus?.Name,
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
            UpdatedAt = lr.UpdatedAt,
            LoadRequestLines = lr.LoadRequestLines?.Select(l => new LoadRequestLineDto
            {
                Id = l.Id,
                LoadRequestId = l.LoadRequestId,
                ProductId = l.ProductId,
                ProductName = l.Product?.Name,
                Quantity = l.Quantity,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                LoadRequestParts = l.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
                {
                    Id = lrp.Id,
                    LoadRequestId = lrp.LoadRequestId,
                    LoadRequestLineId = lrp.LoadRequestLineId,
                    ProductId = lrp.ProductId,
                    ProductName = lrp.Product?.Name ?? l.Product?.Name,
                    PartId = lrp.PartId,
                    PartCode = lrp.Part?.Code,
                    PartName = lrp.Part?.Name,
                    RequiredQuantity = lrp.RequiredQuantity,
                    LoadedQuantity = lrp.LoadedQuantity,
                    CreatedAt = lrp.CreatedAt,
                    UpdatedAt = lrp.UpdatedAt
                }).ToList() ?? new List<LoadRequestPartDto>()
            }).ToList() ?? new List<LoadRequestLineDto>(),
            LoadRequestParts = lr.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
            {
                Id = lrp.Id,
                LoadRequestId = lrp.LoadRequestId,
                LoadRequestLineId = lrp.LoadRequestLineId,
                ProductId = lrp.ProductId,
                ProductName = lrp.Product?.Name,
                PartId = lrp.PartId,
                PartCode = lrp.Part?.Code,
                PartName = lrp.Part?.Name,
                RequiredQuantity = lrp.RequiredQuantity,
                LoadedQuantity = lrp.LoadedQuantity,
                CreatedAt = lrp.CreatedAt,
                UpdatedAt = lrp.UpdatedAt
            }).ToList() ?? new List<LoadRequestPartDto>()
        }).ToList();
    }
}
