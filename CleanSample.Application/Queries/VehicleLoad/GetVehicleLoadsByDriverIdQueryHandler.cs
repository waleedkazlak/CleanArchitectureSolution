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

        return loads.Select(l => new VehicleLoadDto
        {
            Id = l.Id,
            LoadRequestId = l.LoadRequestId,
            PartId = l.PartId,
            PartCode = l.Part?.Code,
            PartName = l.Part?.Name,
            Barcode = l.Barcode,
            Quantity = l.Quantity,
            LoadedBy = l.LoadedBy,
            LoaderName = l.Loader?.FullName,
            DriverId = l.DriverId,
            DriverName = l.Driver?.FullName,
            VehicleId = l.VehicleId,
            VehicleNumber = l.Vehicle?.VehicleNumber,
            PlateNumber = l.Vehicle?.PlateNumber,
            LoadDate = l.LoadDate,
            Status = l.Status,
            Notes = l.Notes,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        });
    }
}
