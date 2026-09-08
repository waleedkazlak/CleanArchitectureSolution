using MediatR;
using CleanSample.Domain.Interfaces;

namespace CleanSample.Application.Commands.Vehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = new CleanSample.Domain.Entities.Vehicle
        {
            VehicleNumber = request.VehicleNumber,
            PlateNumber = request.PlateNumber,
            VehicleType = request.VehicleType,
            CapacityKg = request.CapacityKg,
            IsActive = true
        };

        var vehicleId = await _unitOfWork.Vehicles.AddAsync(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return vehicleId;
    }
}
