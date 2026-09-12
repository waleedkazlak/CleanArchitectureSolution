using CleanSample.Application.Services;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.LoadRequestLine;

public class UpdateLoadRequestLineCommandHandler : IRequestHandler<UpdateLoadRequestLineCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoadRequestBOMService _loadRequestBOMService;
    private readonly ILogger<UpdateLoadRequestLineCommandHandler> _logger;

    public UpdateLoadRequestLineCommandHandler(
        IUnitOfWork unitOfWork,
        ILoadRequestBOMService loadRequestBOMService,
        ILogger<UpdateLoadRequestLineCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _loadRequestBOMService = loadRequestBOMService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateLoadRequestLineCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating LoadRequestLine with Id: {Id}", request.Id);

        var existing = await _unitOfWork.LoadRequestLines.GetByIdAsync(request.Id);
        if (existing == null)
        {
            _logger.LogWarning("LoadRequestLine with Id: {Id} not found", request.Id);
            return false;
        }

        var productChanged = existing.ProductId != request.ProductId;
        var quantityChanged = existing.Quantity != request.Quantity;

        existing.LoadRequestId = request.LoadRequestId;
        existing.ProductId = request.ProductId;
        existing.Quantity = request.Quantity;
        existing.UpdatedAt = DateTime.UtcNow;

        if (productChanged)
        {
            var oldParts = existing.LoadRequestParts.ToList();
            foreach (var oldPart in oldParts)
            {
                await _unitOfWork.LoadRequestParts.DeleteAsync(oldPart.Id);
            }
            existing.LoadRequestParts.Clear();

            var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(request.ProductId);
            foreach (var bom in boms)
            {
                existing.LoadRequestParts.Add(new LoadRequestPart
                {
                    LoadRequestId = existing.LoadRequestId,
                    LoadRequestLineId = existing.Id,
                    LoadRequestLine = existing,
                    ProductId = existing.ProductId,
                    PartId = bom.PartId,
                    RequiredQuantity = request.Quantity * bom.Quantity,
                    LoadedQuantity = 0,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
        else if (quantityChanged)
        {
            var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(request.ProductId);
            var bomDict = boms.ToDictionary(b => b.PartId, b => b.Quantity);

            foreach (var part in existing.LoadRequestParts)
            {
                if (bomDict.TryGetValue(part.PartId, out var bomQty))
                {
                    part.RequiredQuantity = request.Quantity * bomQty;
                    part.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        await _unitOfWork.LoadRequestLines.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Check verification and quantities for associated Order
        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(existing.LoadRequestId);
        if (loadRequest != null && loadRequest.OrderId.HasValue)
        {
            await _loadRequestBOMService.CheckAndGeneratePartsForOrderAsync(loadRequest.OrderId.Value, cancellationToken);
        }

        _logger.LogInformation("LoadRequestLine with Id: {Id} updated successfully", request.Id);
        return true;
    }
}
