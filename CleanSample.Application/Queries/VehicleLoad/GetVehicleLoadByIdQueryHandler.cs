using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.VehicleLoad;

public class GetVehicleLoadByIdQueryHandler : IRequestHandler<GetVehicleLoadByIdQuery, VehicleLoadDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVehicleLoadByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleLoadDto?> Handle(GetVehicleLoadByIdQuery request, CancellationToken cancellationToken)
    {
        var load = await _unitOfWork.VehicleLoads.GetByIdAsync(request.Id);
        if (load == null)
        {
            return null;
        }

        return new VehicleLoadDto
        {
            Id = load.Id,
            LoadRequestId = load.LoadRequestId,
            PartId = load.PartId,
            PartCode = load.Part?.Code,
            PartName = load.Part?.Name,
            Barcode = load.Barcode,
            Quantity = load.Quantity,
            LoadedBy = load.LoadedBy,
            LoaderName = load.Loader?.FullName,
            DriverId = load.DriverId,
            DriverName = load.Driver?.FullName,
            VehicleId = load.VehicleId,
            VehicleNumber = load.Vehicle?.VehicleNumber,
            PlateNumber = load.Vehicle?.PlateNumber,
            LoadDate = load.LoadDate,
            Status = load.Status,
            Notes = load.Notes,
            CreatedAt = load.CreatedAt,
            UpdatedAt = load.UpdatedAt
        };
    }
}
