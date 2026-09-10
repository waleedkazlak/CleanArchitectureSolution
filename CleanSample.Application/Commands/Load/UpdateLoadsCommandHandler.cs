using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Load;

public class UpdateLoadsCommandHandler : IRequestHandler<UpdateLoadsCommand, List<LoadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateLoadsCommandHandler> _logger;

    public UpdateLoadsCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateLoadsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<LoadDto>> Handle(UpdateLoadsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateLoadsCommand with {Count} items", request.Loads?.Count ?? 0);

        if (request.Loads == null || !request.Loads.Any())
        {
            return new List<LoadDto>();
        }

        var updatedLoads = new List<Domain.Entities.Load>();

        foreach (var item in request.Loads)
        {
            var existingLoad = await _unitOfWork.Loads.GetByIdAsync(item.Id);
            if (existingLoad == null)
            {
                _logger.LogWarning("Load with id {LoadId} not found for update", item.Id);
                continue;
            }

            var oldQuantity = existingLoad.Quantity;
            var oldPartId = existingLoad.LoadRequestPartId;

            existingLoad.LoadRequestId = item.LoadRequestId;
            existingLoad.LoadRequestPartId = item.LoadRequestPartId;
            existingLoad.PartId = item.PartId;
            existingLoad.Barcode = item.Barcode;
            existingLoad.Quantity = item.Quantity;
            existingLoad.LoadedBy = item.LoadedBy;
            existingLoad.DriverId = item.DriverId;
            existingLoad.VehicleId = item.VehicleId;
            if (item.LoadDate.HasValue)
            {
                existingLoad.LoadDate = item.LoadDate.Value;
            }
            if (!string.IsNullOrWhiteSpace(item.Status))
            {
                existingLoad.Status = item.Status;
            }
            existingLoad.Notes = item.Notes;
            existingLoad.UpdatedAt = DateTime.UtcNow;

            // Adjust LoadRequestPart LoadedQuantity
            if (oldPartId.HasValue && oldPartId == item.LoadRequestPartId)
            {
                var lrp = await _unitOfWork.LoadRequestParts.GetByIdAsync(oldPartId.Value);
                if (lrp != null)
                {
                    lrp.LoadedQuantity = Math.Max(0, lrp.LoadedQuantity - oldQuantity + item.Quantity);
                    if (lrp.LoadedQuantity >= lrp.RequiredQuantity)
                    {
                        lrp.Status = "Completed";
                    }
                    else if (lrp.LoadedQuantity > 0)
                    {
                        lrp.Status = "PartiallyLoaded";
                    }
                    else
                    {
                        lrp.Status = "Pending";
                    }
                    await _unitOfWork.LoadRequestParts.UpdateAsync(lrp);
                }
            }
            else
            {
                // Removed from old part
                if (oldPartId.HasValue)
                {
                    var oldLrp = await _unitOfWork.LoadRequestParts.GetByIdAsync(oldPartId.Value);
                    if (oldLrp != null)
                    {
                        oldLrp.LoadedQuantity = Math.Max(0, oldLrp.LoadedQuantity - oldQuantity);
                        oldLrp.Status = oldLrp.LoadedQuantity >= oldLrp.RequiredQuantity ? "Completed" :
                                        oldLrp.LoadedQuantity > 0 ? "PartiallyLoaded" : "Pending";
                        await _unitOfWork.LoadRequestParts.UpdateAsync(oldLrp);
                    }
                }
                // Added to new part
                if (item.LoadRequestPartId.HasValue)
                {
                    var newLrp = await _unitOfWork.LoadRequestParts.GetByIdAsync(item.LoadRequestPartId.Value);
                    if (newLrp != null)
                    {
                        newLrp.LoadedQuantity += item.Quantity;
                        newLrp.Status = newLrp.LoadedQuantity >= newLrp.RequiredQuantity ? "Completed" :
                                        newLrp.LoadedQuantity > 0 ? "PartiallyLoaded" : "Pending";
                        await _unitOfWork.LoadRequestParts.UpdateAsync(newLrp);
                    }
                }
            }

            await _unitOfWork.Loads.UpdateAsync(existingLoad);
            updatedLoads.Add(existingLoad);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated {Count} load records", updatedLoads.Count);

        var resultDtos = new List<LoadDto>();
        foreach (var l in updatedLoads)
        {
            var fullLoad = await _unitOfWork.Loads.GetByIdAsync(l.Id);
            if (fullLoad != null)
            {
                resultDtos.Add(new LoadDto
                {
                    Id = fullLoad.Id,
                    LoadRequestId = fullLoad.LoadRequestId,
                    RequestNumber = fullLoad.LoadRequest?.RequestNumber,
                    LoadRequestPartId = fullLoad.LoadRequestPartId,
                    PartId = fullLoad.PartId,
                    PartCode = fullLoad.Part?.Code,
                    PartName = fullLoad.Part?.Name,
                    Barcode = fullLoad.Barcode,
                    Quantity = fullLoad.Quantity,
                    LoadedBy = fullLoad.LoadedBy,
                    LoaderName = fullLoad.Loader?.FullName,
                    DriverId = fullLoad.DriverId,
                    DriverName = fullLoad.Driver?.FullName,
                    VehicleId = fullLoad.VehicleId,
                    VehicleNumber = fullLoad.Vehicle?.VehicleNumber,
                    PlateNumber = fullLoad.Vehicle?.PlateNumber,
                    LoadDate = fullLoad.LoadDate,
                    Status = fullLoad.Status,
                    Notes = fullLoad.Notes,
                    CreatedAt = fullLoad.CreatedAt,
                    UpdatedAt = fullLoad.UpdatedAt
                });
            }
        }

        return resultDtos;
    }
}
