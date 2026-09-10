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
        existingOffload.VehicleId = request.VehicleId;
        existingOffload.DriverId = request.DriverId;
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

        // Synchronize VehicleOffloadItems
        var inputItems = request.VehicleOffloadItems ?? new List<DTOs.VehicleOffloadItemInputDto>();
        var existingItems = existingOffload.VehicleOffloadItems.ToList();

        var inputItemIds = inputItems.Where(i => i.Id.HasValue && i.Id.Value > 0).Select(i => i.Id!.Value).ToHashSet();
        var itemsToDelete = existingItems.Where(e => !inputItemIds.Contains(e.Id)).ToList();

        foreach (var itemToDelete in itemsToDelete)
        {
            existingOffload.VehicleOffloadItems.Remove(itemToDelete);
            await _unitOfWork.VehicleOffloadItems.DeleteAsync(itemToDelete.Id);
        }

        foreach (var inputItem in inputItems)
        {
            if (inputItem.Id.HasValue && inputItem.Id.Value > 0)
            {
                var existingItem = existingItems.FirstOrDefault(e => e.Id == inputItem.Id.Value);
                if (existingItem != null)
                {
                    existingItem.LoadId = inputItem.LoadId;
                    existingItem.PartId = inputItem.PartId;
                    existingItem.Barcode = inputItem.Barcode;
                    existingItem.Quantity = inputItem.Quantity;
                    if (inputItem.OffloadedAt.HasValue)
                    {
                        existingItem.OffloadedAt = inputItem.OffloadedAt.Value;
                    }
                    existingItem.UpdatedAt = DateTime.UtcNow;
                    await _unitOfWork.VehicleOffloadItems.UpdateAsync(existingItem);
                }
            }
            else
            {
                var newItem = new VehicleOffloadItem
                {
                    VehicleOffloadId = existingOffload.Id,
                    LoadId = inputItem.LoadId,
                    PartId = inputItem.PartId,
                    Barcode = inputItem.Barcode,
                    Quantity = inputItem.Quantity,
                    OffloadedAt = inputItem.OffloadedAt ?? DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                existingOffload.VehicleOffloadItems.Add(newItem);
                await _unitOfWork.VehicleOffloadItems.AddAsync(newItem);
            }
        }

        await _unitOfWork.VehicleOffloads.UpdateAsync(existingOffload);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated VehicleOffload with ID: {VehicleOffloadId}", existingOffload.Id);
        return true;
    }
}
