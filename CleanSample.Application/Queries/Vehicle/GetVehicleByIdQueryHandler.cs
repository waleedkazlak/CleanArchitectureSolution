using MediatR;
using CleanSample.Domain.Interfaces;
using CleanSample.Application.DTOs;

namespace CleanSample.Application.Queries.Vehicle;

public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVehicleByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.Id);
        if (vehicle == null)
        {
            return null;
        }

        return new VehicleDto
        {
            Id = vehicle.Id,
            VehicleNumber = vehicle.VehicleNumber,
            PlateNumber = vehicle.PlateNumber,
            VehicleType = vehicle.VehicleType,
            CapacityKg = vehicle.CapacityKg,
            IsActive = vehicle.IsActive,
            CreatedAt = vehicle.CreatedAt,
            UpdatedAt = vehicle.UpdatedAt
        };
    }
}
