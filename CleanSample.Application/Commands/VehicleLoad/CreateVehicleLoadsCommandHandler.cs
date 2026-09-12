using CleanSample.Application.DTOs;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleLoad;

public class CreateVehicleLoadsCommandHandler : IRequestHandler<CreateVehicleLoadsCommand, List<VehicleLoadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVehicleLoadsCommandHandler> _logger;
    private readonly Services.IWorkflowOrchestratorService _workflowOrchestrator;

    public CreateVehicleLoadsCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateVehicleLoadsCommandHandler> logger,
        Services.IWorkflowOrchestratorService workflowOrchestrator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _workflowOrchestrator = workflowOrchestrator;
    }

    public async Task<List<VehicleLoadDto>> Handle(CreateVehicleLoadsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateVehicleLoadsCommand with {Count} items", request.Loads?.Count ?? 0);

        if (request.Loads == null || !request.Loads.Any())
        {
            return new List<VehicleLoadDto>();
        }

        var newLoads = new List<Domain.Entities.VehicleLoad>();

        foreach (var item in request.Loads)
        {
            var load = new Domain.Entities.VehicleLoad
            {
                LoadRequestId = item.LoadRequestId,
                PartId = item.PartId,
                Barcode = item.Barcode,
                Quantity = item.Quantity,
                LoadedBy = item.LoadedBy,
                DriverId = item.DriverId,
                VehicleId = item.VehicleId,
                LoadDate = item.LoadDate ?? DateTime.UtcNow,
                Status = item.Status ?? (int)Domain.Enums.VehicleLoadStatusEnum.Good,
                Notes = item.Notes,
                CreatedAt = DateTime.UtcNow
            };

            newLoads.Add(load);

            // Update LoadRequestPart LoadedQuantity if matching part exists
            var relatedParts = await _unitOfWork.LoadRequestParts.GetByLoadRequestIdAsync(item.LoadRequestId);
            var lrp = relatedParts.FirstOrDefault(p => p.PartId == item.PartId);
            if (lrp != null)
            {
                lrp.LoadedQuantity += item.Quantity;
                await _unitOfWork.LoadRequestParts.UpdateAsync(lrp);
            }
        }

        await _unitOfWork.VehicleLoads.AddRangeAsync(newLoads);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Trigger workflow recalculation for each affected LoadRequest
        var affectedLoadRequestIds = newLoads.Select(l => l.LoadRequestId).Distinct();
        foreach (var loadRequestId in affectedLoadRequestIds)
        {
            await _workflowOrchestrator.RecalculateLoadRequestStatusAfterVehicleLoadsChangeAsync(loadRequestId, cancellationToken);
        }

        _logger.LogInformation("Successfully created {Count} vehicle load records", newLoads.Count);

        // Map to DTOs
        var resultDtos = new List<VehicleLoadDto>();
        foreach (var l in newLoads)
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
            else
            {
                resultDtos.Add(new VehicleLoadDto
                {
                    Id = l.Id,
                    LoadRequestId = l.LoadRequestId,
                    PartId = l.PartId,
                    Barcode = l.Barcode,
                    Quantity = l.Quantity,
                    LoadedBy = l.LoadedBy,
                    DriverId = l.DriverId,
                    VehicleId = l.VehicleId,
                    LoadDate = l.LoadDate,
                    Status = l.Status,
                    Notes = l.Notes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                });
            }
        }

        return resultDtos;
    }
}
