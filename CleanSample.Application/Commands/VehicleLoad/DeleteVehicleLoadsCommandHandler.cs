using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleLoad;

public class DeleteVehicleLoadsCommandHandler : IRequestHandler<DeleteVehicleLoadsCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteVehicleLoadsCommandHandler> _logger;

    public DeleteVehicleLoadsCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteVehicleLoadsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteVehicleLoadsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteVehicleLoadsCommand for {Count} ids", request.LoadIds?.Count ?? 0);

        if (request.LoadIds == null || !request.LoadIds.Any())
        {
            return false;
        }

        foreach (var id in request.LoadIds)
        {
            var load = await _unitOfWork.VehicleLoads.GetByIdAsync(id);
            if (load != null)
            {
                var relatedParts = await _unitOfWork.LoadRequestParts.GetByLoadRequestIdAsync(load.LoadRequestId);
                var lrp = relatedParts.FirstOrDefault(p => p.PartId == load.PartId);
                if (lrp != null)
                {
                    lrp.LoadedQuantity = Math.Max(0, lrp.LoadedQuantity - load.Quantity);
                    lrp.Status = lrp.LoadedQuantity >= lrp.RequiredQuantity ? "Completed" :
                                 lrp.LoadedQuantity > 0 ? "PartiallyLoaded" : "Pending";
                    await _unitOfWork.LoadRequestParts.UpdateAsync(lrp);
                }
                await _unitOfWork.VehicleLoads.DeleteAsync(id);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Successfully deleted vehicle loads: {Ids}", string.Join(",", request.LoadIds));

        return true;
    }
}
