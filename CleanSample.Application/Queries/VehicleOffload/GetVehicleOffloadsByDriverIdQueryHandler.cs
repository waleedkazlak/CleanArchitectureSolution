using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.VehicleOffload;

public class GetVehicleOffloadsByDriverIdQueryHandler : IRequestHandler<GetVehicleOffloadsByDriverIdQuery, IEnumerable<VehicleOffloadDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVehicleOffloadsByDriverIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VehicleOffloadDto>> Handle(GetVehicleOffloadsByDriverIdQuery request, CancellationToken cancellationToken)
    {
        var offloads = await _unitOfWork.VehicleOffloads.GetByDriverIdAsync(request.DriverId);

        return offloads.Select(vo => new VehicleOffloadDto
        {
            Id = vo.Id,
            LoadRequestId = vo.LoadRequestId,
            PartId = vo.PartId,
            PartCode = vo.Part?.Code,
            PartName = vo.Part?.Name,
            VehicleId = vo.VehicleId,
            VehicleNumber = vo.Vehicle?.VehicleNumber,
            PlateNumber = vo.Vehicle?.PlateNumber,
            DriverId = vo.DriverId,
            DriverName = vo.Driver?.FullName,
            Barcode = vo.Barcode,
            OffloadDate = vo.OffloadDate,
            Status = vo.Status,
            Verified = vo.Verified,
            VerifiedBy = vo.VerifiedBy,
            VerifierName = vo.Verifier?.FullName,
            VerifiedAt = vo.VerifiedAt,
            Notes = vo.Notes,
            CreatedAt = vo.CreatedAt,
            UpdatedAt = vo.UpdatedAt
        });
    }
}
