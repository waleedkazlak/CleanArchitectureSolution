using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequest;

public class GetLoadRequestByIdQueryHandler : IRequestHandler<GetLoadRequestByIdQuery, LoadRequestDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLoadRequestByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LoadRequestDto?> Handle(GetLoadRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var lr = await _unitOfWork.LoadRequests.GetByIdAsync(request.Id);
        if (lr == null)
        {
            return null;
        }

        var lines = lr.LoadRequestLines?.Select(l => new LoadRequestLineDto
        {
            Id = l.Id,
            LoadRequestId = l.LoadRequestId,
            RequestNumber = lr.RequestNumber,
            ProductVariantId = l.ProductVariantId,
            ProductVariantCode = l.ProductVariant?.Code,
            Quantity = l.Quantity,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt,
            LoadRequestParts = l.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
            {
                Id = lrp.Id,
                LoadRequestId = lrp.LoadRequestId,
                LoadRequestLineId = lrp.LoadRequestLineId,
                PartId = lrp.PartId,
                PartCode = lrp.Part?.Code,
                PartName = lrp.Part?.Name,
                RequiredQuantity = lrp.RequiredQuantity,
                LoadedQuantity = lrp.LoadedQuantity,
                Status = lrp.Status,
                CreatedAt = lrp.CreatedAt,
                UpdatedAt = lrp.UpdatedAt
            }).ToList() ?? new List<LoadRequestPartDto>()
        }).ToList() ?? new List<LoadRequestLineDto>();

        var allParts = lr.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
        {
            Id = lrp.Id,
            LoadRequestId = lrp.LoadRequestId,
            LoadRequestLineId = lrp.LoadRequestLineId,
            PartId = lrp.PartId,
            PartCode = lrp.Part?.Code,
            PartName = lrp.Part?.Name,
            RequiredQuantity = lrp.RequiredQuantity,
            LoadedQuantity = lrp.LoadedQuantity,
            Status = lrp.Status,
            CreatedAt = lrp.CreatedAt,
            UpdatedAt = lrp.UpdatedAt
        }).ToList() ?? new List<LoadRequestPartDto>();

        return new LoadRequestDto
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
            UpdatedAt = lr.UpdatedAt,
            LoadRequestLines = lines,
            LoadRequestParts = allParts
        };
    }
}
