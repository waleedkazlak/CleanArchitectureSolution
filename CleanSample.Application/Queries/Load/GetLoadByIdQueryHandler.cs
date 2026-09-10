using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Load;

public class GetLoadByIdQueryHandler : IRequestHandler<GetLoadByIdQuery, LoadDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLoadByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LoadDto?> Handle(GetLoadByIdQuery request, CancellationToken cancellationToken)
    {
        var load = await _unitOfWork.Loads.GetByIdAsync(request.Id);
        if (load == null)
        {
            return null;
        }

        return new LoadDto
        {
            Id = load.Id,
            LoadRequestId = load.LoadRequestId,
            RequestNumber = load.LoadRequest?.RequestNumber,
            LoadRequestPartId = load.LoadRequestPartId,
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
