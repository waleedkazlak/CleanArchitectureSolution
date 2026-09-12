using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleLoad;

public class DeleteVehicleLoadsCommandHandler : IRequestHandler<DeleteVehicleLoadsCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteVehicleLoadsCommandHandler> _logger;
    private readonly Services.IWorkflowOrchestratorService _workflowOrchestrator;

    public DeleteVehicleLoadsCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteVehicleLoadsCommandHandler> logger,
        Services.IWorkflowOrchestratorService workflowOrchestrator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _workflowOrchestrator = workflowOrchestrator;
    }

    public async Task<bool> Handle(DeleteVehicleLoadsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteVehicleLoadsCommand for {Count} ids", request.LoadIds?.Count ?? 0);

        if (request.LoadIds == null || !request.LoadIds.Any())
        {
            return false;
        }

        var affectedLoadRequestIds = new HashSet<long>();

        foreach (var id in request.LoadIds)
        {
            var load = await _unitOfWork.VehicleLoads.GetByIdAsync(id);
            if (load != null)
            {
                affectedLoadRequestIds.Add(load.LoadRequestId);
                var relatedParts = await _unitOfWork.LoadRequestParts.GetByLoadRequestIdAsync(load.LoadRequestId);
                var lrp = relatedParts.FirstOrDefault(p => p.PartId == load.PartId);
                if (lrp != null)
                {
                    lrp.LoadedQuantity = Math.Max(0, lrp.LoadedQuantity - load.Quantity);
                    await _unitOfWork.LoadRequestParts.UpdateAsync(lrp);
                }
                await _unitOfWork.VehicleLoads.DeleteAsync(id);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var loadRequestId in affectedLoadRequestIds)
        {
            await _workflowOrchestrator.RecalculateLoadRequestStatusAfterVehicleLoadsChangeAsync(loadRequestId, cancellationToken);
        }

        _logger.LogInformation("Successfully deleted vehicle loads: {Ids}", string.Join(",", request.LoadIds));

        return true;
    }
}
