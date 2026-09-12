using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleLoad;

public class UpdateVehicleLoadsCommandHandler : IRequestHandler<UpdateVehicleLoadsCommand, List<VehicleLoadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateVehicleLoadsCommandHandler> _logger;
    private readonly Services.IWorkflowOrchestratorService _workflowOrchestrator;

    public UpdateVehicleLoadsCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateVehicleLoadsCommandHandler> logger,
        Services.IWorkflowOrchestratorService workflowOrchestrator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _workflowOrchestrator = workflowOrchestrator;
    }

    public async Task<List<VehicleLoadDto>> Handle(UpdateVehicleLoadsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateVehicleLoadsCommand with {Count} items", request.Loads?.Count ?? 0);

        if (request.Loads == null || !request.Loads.Any())
        {
            return new List<VehicleLoadDto>();
        }

        var updatedLoads = new List<Domain.Entities.VehicleLoad>();

        foreach (var item in request.Loads)
        {
            var existingLoad = await _unitOfWork.VehicleLoads.GetByIdAsync(item.Id);
            if (existingLoad == null)
            {
                _logger.LogWarning("VehicleLoad with id {LoadId} not found for update", item.Id);
                continue;
            }

            var oldQuantity = existingLoad.Quantity;
            var oldPartId = existingLoad.PartId;
            var oldLoadRequestId = existingLoad.LoadRequestId;

            existingLoad.LoadRequestId = item.LoadRequestId;
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
            if (item.Status.HasValue)
            {
                existingLoad.Status = item.Status.Value;
            }
            existingLoad.Notes = item.Notes;
            existingLoad.UpdatedAt = DateTime.UtcNow;

            // Adjust LoadRequestPart LoadedQuantity
            if (oldLoadRequestId == item.LoadRequestId && oldPartId == item.PartId)
            {
                var relatedParts = await _unitOfWork.LoadRequestParts.GetByLoadRequestIdAsync(item.LoadRequestId);
                var lrp = relatedParts.FirstOrDefault(p => p.PartId == item.PartId);
                if (lrp != null)
                {
                    lrp.LoadedQuantity = Math.Max(0, lrp.LoadedQuantity - oldQuantity + item.Quantity);
                    await _unitOfWork.LoadRequestParts.UpdateAsync(lrp);
                }
            }
            else
            {
                // Removed from old part
                var oldParts = await _unitOfWork.LoadRequestParts.GetByLoadRequestIdAsync(oldLoadRequestId);
                var oldLrp = oldParts.FirstOrDefault(p => p.PartId == oldPartId);
                if (oldLrp != null)
                {
                    oldLrp.LoadedQuantity = Math.Max(0, oldLrp.LoadedQuantity - oldQuantity);
                    await _unitOfWork.LoadRequestParts.UpdateAsync(oldLrp);
                }

                // Added to new part
                var newParts = await _unitOfWork.LoadRequestParts.GetByLoadRequestIdAsync(item.LoadRequestId);
                var newLrp = newParts.FirstOrDefault(p => p.PartId == item.PartId);
                if (newLrp != null)
                {
                    newLrp.LoadedQuantity += item.Quantity;
                    await _unitOfWork.LoadRequestParts.UpdateAsync(newLrp);
                }
            }

            await _unitOfWork.VehicleLoads.UpdateAsync(existingLoad);
            updatedLoads.Add(existingLoad);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Trigger workflow recalculation for each affected LoadRequest
        var affectedLoadRequestIds = updatedLoads.Select(l => l.LoadRequestId).Distinct();
        foreach (var loadRequestId in affectedLoadRequestIds)
        {
            await _workflowOrchestrator.RecalculateLoadRequestStatusAfterVehicleLoadsChangeAsync(loadRequestId, cancellationToken);
        }

        _logger.LogInformation("Successfully updated {Count} vehicle load records", updatedLoads.Count);

        var resultDtos = new List<VehicleLoadDto>();
        foreach (var l in updatedLoads)
        {
            var fullLoad = await _unitOfWork.VehicleLoads.GetByIdAsync(l.Id);
            if (fullLoad != null)
            {
                resultDtos.Add(new VehicleLoadDto
                {
                    Id = fullLoad.Id,
                    LoadRequestId = fullLoad.LoadRequestId,
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
                    StatusName = fullLoad.VehicleLoadStatus?.Name,
                    Notes = fullLoad.Notes,
                    CreatedAt = fullLoad.CreatedAt,
                    UpdatedAt = fullLoad.UpdatedAt
                });
            }
        }

        return resultDtos;
    }
}
