using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Pick;

public class GetPicksByDriverIdQueryHandler : IRequestHandler<GetPicksByDriverIdQuery, IEnumerable<PickDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPicksByDriverIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PickDto>> Handle(GetPicksByDriverIdQuery request, CancellationToken cancellationToken)
    {
        var picks = await _unitOfWork.Picks.GetByDriverIdAsync(request.DriverId);

        return picks.Select(p => new PickDto
        {
            Id = p.Id,
            PickRequestId = p.PickRequestId,
            RequestNumber = p.PickRequest?.RequestNumber,
            PickRequestPartId = p.PickRequestPartId,
            PartId = p.PartId,
            PartCode = p.Part?.Code,
            PartName = p.Part?.Name,
            Barcode = p.Barcode,
            Quantity = p.Quantity,
            PickedBy = p.PickedBy,
            PickerName = p.Picker?.FullName,
            DriverId = p.DriverId,
            DriverName = p.Driver?.FullName,
            VehicleId = p.VehicleId,
            VehicleNumber = p.Vehicle?.VehicleNumber,
            PlateNumber = p.Vehicle?.PlateNumber,
            PickDate = p.PickDate,
            Status = p.Status,
            Notes = p.Notes,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });
    }
}
