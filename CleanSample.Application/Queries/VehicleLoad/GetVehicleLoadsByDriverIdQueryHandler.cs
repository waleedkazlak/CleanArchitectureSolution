using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.VehicleLoad;

public class GetVehicleLoadsByDriverIdQueryHandler : IRequestHandler<GetVehicleLoadsByDriverIdQuery, IEnumerable<VehicleLoadDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVehicleLoadsByDriverIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VehicleLoadDto>> Handle(GetVehicleLoadsByDriverIdQuery request, CancellationToken cancellationToken)
    {
        var loads = await _unitOfWork.VehicleLoads.GetByDriverIdAsync(request.DriverId);

        return loads.Select(vl => new VehicleLoadDto
        {
            Id = vl.Id,
            PickRequestId = vl.PickRequestId,
            RequestNumber = vl.PickRequest?.RequestNumber,
            VehicleId = vl.VehicleId,
            VehicleNumber = vl.Vehicle?.VehicleNumber,
            PlateNumber = vl.Vehicle?.PlateNumber,
            DriverId = vl.DriverId,
            DriverName = vl.Driver?.FullName,
            LoadDate = vl.LoadDate,
            Status = vl.Status,
            Verified = vl.Verified,
            VerifiedBy = vl.VerifiedBy,
            VerifierName = vl.Verifier?.FullName,
            VerifiedAt = vl.VerifiedAt,
            Notes = vl.Notes,
            CreatedAt = vl.CreatedAt,
            UpdatedAt = vl.UpdatedAt,
            VehicleLoadItems = vl.VehicleLoadItems?.Select(i => new VehicleLoadItemDto
            {
                Id = i.Id,
                VehicleLoadId = i.VehicleLoadId,
                PickId = i.PickId,
                PartId = i.PartId,
                PartCode = i.Part?.Code,
                PartName = i.Part?.Name,
                Barcode = i.Barcode,
                Quantity = i.Quantity,
                LoadedAt = i.LoadedAt,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            }).ToList() ?? new List<VehicleLoadItemDto>()
        });
    }
}
