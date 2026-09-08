using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Pick;

public class GetPickByIdQueryHandler : IRequestHandler<GetPickByIdQuery, PickDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPickByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PickDto?> Handle(GetPickByIdQuery request, CancellationToken cancellationToken)
    {
        var pick = await _unitOfWork.Picks.GetByIdAsync(request.Id);
        if (pick == null)
        {
            return null;
        }

        return new PickDto
        {
            Id = pick.Id,
            PickRequestId = pick.PickRequestId,
            RequestNumber = pick.PickRequest?.RequestNumber,
            PickRequestPartId = pick.PickRequestPartId,
            PartId = pick.PartId,
            PartCode = pick.Part?.Code,
            PartName = pick.Part?.Name,
            Barcode = pick.Barcode,
            Quantity = pick.Quantity,
            PickedBy = pick.PickedBy,
            PickerName = pick.Picker?.FullName,
            DriverId = pick.DriverId,
            DriverName = pick.Driver?.FullName,
            VehicleId = pick.VehicleId,
            VehicleNumber = pick.Vehicle?.VehicleNumber,
            PlateNumber = pick.Vehicle?.PlateNumber,
            PickDate = pick.PickDate,
            Status = pick.Status,
            Notes = pick.Notes,
            CreatedAt = pick.CreatedAt,
            UpdatedAt = pick.UpdatedAt
        };
    }
}
