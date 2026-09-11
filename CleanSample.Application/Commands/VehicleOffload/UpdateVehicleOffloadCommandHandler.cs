using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleOffload;

public class UpdateVehicleOffloadCommandHandler : IRequestHandler<UpdateVehicleOffloadCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateVehicleOffloadCommandHandler> _logger;

    public UpdateVehicleOffloadCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateVehicleOffloadCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateVehicleOffloadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateVehicleOffloadCommand for ID: {VehicleOffloadId}", request.Id);

        var existingOffload = await _unitOfWork.VehicleOffloads.GetByIdAsync(request.Id);
        if (existingOffload == null)
        {
            _logger.LogWarning("VehicleOffload with ID {VehicleOffloadId} not found", request.Id);
            return false;
        }

        existingOffload.LoadRequestId = request.LoadRequestId;
        existingOffload.PartId = request.PartId;
        existingOffload.VehicleId = request.VehicleId;
        existingOffload.DriverId = request.DriverId;
        existingOffload.Barcode = request.Barcode;
        if (request.OffloadDate.HasValue)
        {
            existingOffload.OffloadDate = request.OffloadDate.Value;
        }
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            existingOffload.Status = request.Status;
        }
        existingOffload.Verified = request.Verified;
        existingOffload.VerifiedBy = request.VerifiedBy;
        existingOffload.VerifiedAt = request.Verified ? (request.VerifiedAt ?? DateTime.UtcNow) : null;
        existingOffload.Notes = request.Notes;
        existingOffload.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.VehicleOffloads.UpdateAsync(existingOffload);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated VehicleOffload with ID: {VehicleOffloadId}", existingOffload.Id);
        return true;
    }
}
