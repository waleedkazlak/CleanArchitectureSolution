using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleLoad;

public class UpdateVehicleLoadCommandHandler : IRequestHandler<UpdateVehicleLoadCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateVehicleLoadCommandHandler> _logger;

    public UpdateVehicleLoadCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateVehicleLoadCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateVehicleLoadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateVehicleLoadCommand for ID: {VehicleLoadId}", request.Id);

        var existingLoad = await _unitOfWork.VehicleLoads.GetByIdAsync(request.Id);
        if (existingLoad == null)
        {
            _logger.LogWarning("VehicleLoad with ID {VehicleLoadId} not found", request.Id);
            return false;
        }

        existingLoad.PickRequestId = request.PickRequestId;
        existingLoad.VehicleId = request.VehicleId;
        existingLoad.DriverId = request.DriverId;
        if (request.LoadDate.HasValue)
        {
            existingLoad.LoadDate = request.LoadDate.Value;
        }
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            existingLoad.Status = request.Status;
        }
        existingLoad.Verified = request.Verified;
        existingLoad.VerifiedBy = request.VerifiedBy;
        existingLoad.VerifiedAt = request.Verified ? (request.VerifiedAt ?? DateTime.UtcNow) : null;
        existingLoad.Notes = request.Notes;
        existingLoad.UpdatedAt = DateTime.UtcNow;

        // Synchronize VehicleLoadItems
        var inputItems = request.VehicleLoadItems ?? new List<DTOs.VehicleLoadItemInputDto>();
        var existingItems = existingLoad.VehicleLoadItems.ToList();

        var inputItemIds = inputItems.Where(i => i.Id.HasValue && i.Id.Value > 0).Select(i => i.Id!.Value).ToHashSet();
        var itemsToDelete = existingItems.Where(e => !inputItemIds.Contains(e.Id)).ToList();

        foreach (var itemToDelete in itemsToDelete)
        {
            existingLoad.VehicleLoadItems.Remove(itemToDelete);
            await _unitOfWork.VehicleLoadItems.DeleteAsync(itemToDelete.Id);
        }

        foreach (var inputItem in inputItems)
        {
            if (inputItem.Id.HasValue && inputItem.Id.Value > 0)
            {
                var existingItem = existingItems.FirstOrDefault(e => e.Id == inputItem.Id.Value);
                if (existingItem != null)
                {
                    existingItem.PickId = inputItem.PickId;
                    existingItem.PartId = inputItem.PartId;
                    existingItem.Barcode = inputItem.Barcode;
                    existingItem.Quantity = inputItem.Quantity;
                    if (inputItem.LoadedAt.HasValue)
                    {
                        existingItem.LoadedAt = inputItem.LoadedAt.Value;
                    }
                    existingItem.UpdatedAt = DateTime.UtcNow;
                    await _unitOfWork.VehicleLoadItems.UpdateAsync(existingItem);
                }
            }
            else
            {
                var newItem = new VehicleLoadItem
                {
                    VehicleLoadId = existingLoad.Id,
                    PickId = inputItem.PickId,
                    PartId = inputItem.PartId,
                    Barcode = inputItem.Barcode,
                    Quantity = inputItem.Quantity,
                    LoadedAt = inputItem.LoadedAt ?? DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                existingLoad.VehicleLoadItems.Add(newItem);
                await _unitOfWork.VehicleLoadItems.AddAsync(newItem);
            }
        }

        await _unitOfWork.VehicleLoads.UpdateAsync(existingLoad);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated VehicleLoad with ID: {VehicleLoadId}", existingLoad.Id);
        return true;
    }
}
