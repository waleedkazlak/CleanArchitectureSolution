using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Load;

public class GetLoadsByLoadRequestIdQueryHandler : IRequestHandler<GetLoadsByLoadRequestIdQuery, IEnumerable<LoadDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLoadsByLoadRequestIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<LoadDto>> Handle(GetLoadsByLoadRequestIdQuery request, CancellationToken cancellationToken)
    {
        var loads = await _unitOfWork.Loads.GetByLoadRequestIdAsync(request.LoadRequestId);

        return loads.Select(l => new LoadDto
        {
            Id = l.Id,
            LoadRequestId = l.LoadRequestId,
            RequestNumber = l.LoadRequest?.RequestNumber,
            LoadRequestPartId = l.LoadRequestPartId,
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
