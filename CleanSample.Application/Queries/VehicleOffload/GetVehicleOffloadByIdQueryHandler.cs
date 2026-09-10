using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.VehicleOffload;

public class GetVehicleOffloadByIdQueryHandler : IRequestHandler<GetVehicleOffloadByIdQuery, VehicleOffloadDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVehicleOffloadByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleOffloadDto?> Handle(GetVehicleOffloadByIdQuery request, CancellationToken cancellationToken)
    {
        var vo = await _unitOfWork.VehicleOffloads.GetByIdAsync(request.Id);
        if (vo == null)
        {
            return null;
        }

        return new VehicleOffloadDto
        {
            Id = vo.Id,
            LoadRequestId = vo.LoadRequestId,
            RequestNumber = vo.LoadRequest?.RequestNumber,
            VehicleId = vo.VehicleId,
            VehicleNumber = vo.Vehicle?.VehicleNumber,
            PlateNumber = vo.Vehicle?.PlateNumber,
            DriverId = vo.DriverId,
            DriverName = vo.Driver?.FullName,
            OffloadDate = vo.OffloadDate,
            Status = vo.Status,
            Verified = vo.Verified,
            VerifiedBy = vo.VerifiedBy,
            VerifierName = vo.Verifier?.FullName,
            VerifiedAt = vo.VerifiedAt,
            Notes = vo.Notes,
            CreatedAt = vo.CreatedAt,
            UpdatedAt = vo.UpdatedAt,
            VehicleOffloadItems = vo.VehicleOffloadItems?.Select(i => new VehicleOffloadItemDto
            {
                Id = i.Id,
                VehicleOffloadId = i.VehicleOffloadId,
                LoadId = i.LoadId,
                PartId = i.PartId,
                PartCode = i.Part?.Code,
                PartName = i.Part?.Name,
                Barcode = i.Barcode,
                Quantity = i.Quantity,
                OffloadedAt = i.OffloadedAt,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            }).ToList() ?? new List<VehicleOffloadItemDto>()
        };
    }
}
