using MediatR;
using CleanSample.Domain.Interfaces;

namespace CleanSample.Application.Commands.Vehicle;

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.Id);
        if (vehicle == null)
        {
            return false;
        }

        vehicle.VehicleNumber = request.VehicleNumber;
        vehicle.PlateNumber = request.PlateNumber;
        vehicle.VehicleType = request.VehicleType;
        vehicle.CapacityKg = request.CapacityKg;
        vehicle.IsActive = request.IsActive;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Vehicles.UpdateAsync(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
