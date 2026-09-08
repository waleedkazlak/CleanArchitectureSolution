using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.PickRequest;

public class GetPickRequestByIdQueryHandler : IRequestHandler<GetPickRequestByIdQuery, PickRequestDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPickRequestByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PickRequestDto?> Handle(GetPickRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var p = await _unitOfWork.PickRequests.GetByIdAsync(request.Id);
        if (p == null)
        {
            return null;
        }

        return new PickRequestDto
        {
            Id = p.Id,
            RequestNumber = p.RequestNumber,
            OrderId = p.OrderId,
            OrderNumber = p.Order?.OrderNumber,
            ClientId = p.ClientId,
            ClientName = p.Client?.Name,
            ClientLocationId = p.ClientLocationId,
            ClientLocationName = p.ClientLocation?.Name,
            RequestedBy = p.RequestedBy,
            RequesterName = p.Requester?.FullName,
            RequestDate = p.RequestDate,
            ExecutionDate = p.ExecutionDate,
            Status = p.Status,
            DestinationAddress = p.DestinationAddress,
            DestinationCity = p.DestinationCity,
            Description = p.Description,
            DriverId = p.DriverId,
            DriverName = p.Driver?.FullName,
            VehicleId = p.VehicleId,
            VehicleNumber = p.Vehicle?.VehicleNumber,
            PlateNumber = p.Vehicle?.PlateNumber,
            Verified = p.Verified,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            PickRequestLines = p.PickRequestLines?.Select(l => new PickRequestLineDto
            {
                Id = l.Id,
                PickRequestId = l.PickRequestId,
                RequestNumber = p.RequestNumber,
                ProductVariantId = l.ProductVariantId,
                ProductVariantCode = l.ProductVariant?.Code,
                Quantity = l.Quantity,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            }).ToList() ?? new List<PickRequestLineDto>()
        };
    }
}
