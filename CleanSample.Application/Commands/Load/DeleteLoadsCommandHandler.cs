using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Load;

public class DeleteLoadsCommandHandler : IRequestHandler<DeleteLoadsCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLoadsCommandHandler> _logger;

    public DeleteLoadsCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteLoadsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteLoadsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteLoadsCommand for {Count} ids", request.LoadIds?.Count ?? 0);

        if (request.LoadIds == null || !request.LoadIds.Any())
        {
            return false;
        }

        foreach (var id in request.LoadIds)
        {
            var load = await _unitOfWork.Loads.GetByIdAsync(id);
            if (load != null)
            {
                if (load.LoadRequestPartId.HasValue)
                {
                    var lrp = await _unitOfWork.LoadRequestParts.GetByIdAsync(load.LoadRequestPartId.Value);
                    if (lrp != null)
                    {
                        lrp.LoadedQuantity = Math.Max(0, lrp.LoadedQuantity - load.Quantity);
                        lrp.Status = lrp.LoadedQuantity >= lrp.RequiredQuantity ? "Completed" :
                                     lrp.LoadedQuantity > 0 ? "PartiallyLoaded" : "Pending";
                        await _unitOfWork.LoadRequestParts.UpdateAsync(lrp);
                    }
                }
                await _unitOfWork.Loads.DeleteAsync(id);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Successfully deleted loads: {Ids}", string.Join(",", request.LoadIds));

        return true;
    }
}
