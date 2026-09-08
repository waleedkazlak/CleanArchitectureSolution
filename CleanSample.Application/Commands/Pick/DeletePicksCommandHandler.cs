using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Pick;

public class DeletePicksCommandHandler : IRequestHandler<DeletePicksCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePicksCommandHandler> _logger;

    public DeletePicksCommandHandler(IUnitOfWork unitOfWork, ILogger<DeletePicksCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeletePicksCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeletePicksCommand for {Count} ids", request.PickIds?.Count ?? 0);

        if (request.PickIds == null || !request.PickIds.Any())
        {
            return false;
        }

        foreach (var id in request.PickIds)
        {
            var pick = await _unitOfWork.Picks.GetByIdAsync(id);
            if (pick != null)
            {
                if (pick.PickRequestPartId.HasValue)
                {
                    var prp = await _unitOfWork.PickRequestParts.GetByIdAsync(pick.PickRequestPartId.Value);
                    if (prp != null)
                    {
                        prp.PickedQuantity = Math.Max(0, prp.PickedQuantity - pick.Quantity);
                        prp.Status = prp.PickedQuantity >= prp.RequiredQuantity ? "Completed" :
                                     prp.PickedQuantity > 0 ? "PartiallyPicked" : "Pending";
                        await _unitOfWork.PickRequestParts.UpdateAsync(prp);
                    }
                }
                await _unitOfWork.Picks.DeleteAsync(id);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Successfully deleted picks: {Ids}", string.Join(",", request.PickIds));

        return true;
    }
}
