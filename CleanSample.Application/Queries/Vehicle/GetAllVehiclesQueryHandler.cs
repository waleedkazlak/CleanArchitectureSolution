using MediatR;
using CleanSample.Domain.Interfaces;
using CleanSample.Application.DTOs;

namespace CleanSample.Application.Queries.Vehicle;

public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, IEnumerable<VehicleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllVehiclesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _unitOfWork.Vehicles.GetAllAsync();
        
        return vehicles.Select(vehicle => new VehicleDto
        {
            Id = vehicle.Id,
            VehicleNumber = vehicle.VehicleNumber,
            PlateNumber = vehicle.PlateNumber,
            VehicleType = vehicle.VehicleType,
            CapacityKg = vehicle.CapacityKg,
            IsActive = vehicle.IsActive,
            CreatedAt = vehicle.CreatedAt,
            UpdatedAt = vehicle.UpdatedAt
        });
    }
}
