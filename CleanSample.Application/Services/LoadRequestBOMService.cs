using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Services;

public class LoadRequestBOMService : ILoadRequestBOMService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoadRequestBOMService> _logger;

    public LoadRequestBOMService(
        IUnitOfWork unitOfWork,
        ILogger<LoadRequestBOMService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> CheckAndGeneratePartsForOrderAsync(long orderId, CancellationToken cancellationToken = default)
    {
        if (orderId <= 0)
        {
            return false;
        }

        _logger.LogInformation("Checking load requests verification and quantities for Order ID: {OrderId}", orderId);

        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null || order.OrderLines == null || !order.OrderLines.Any())
        {
            _logger.LogWarning("Order {OrderId} not found or contains no order lines", orderId);
            return false;
        }

        var loadRequests = (await _unitOfWork.LoadRequests.GetByOrderIdAsync(orderId)).ToList();
        if (!loadRequests.Any())
        {
            _logger.LogInformation("No LoadRequests found for Order ID: {OrderId}", orderId);
            return false;
        }

        // Condition 1: All LoadRequests for this order must be verified
        var unverifiedRequests = loadRequests.Where(lr => !lr.Verified).ToList();
        if (unverifiedRequests.Any())
        {
            _logger.LogInformation(
                "Order {OrderId} has {Count} unverified load request(s) (IDs: {Ids}). BOM parts generation skipped.",
                orderId,
                unverifiedRequests.Count,
                string.Join(", ", unverifiedRequests.Select(u => u.Id)));
            return false;
        }

        // Condition 2: All ordered products quantities must exist and match order quantities
        var orderedQuantities = order.OrderLines
            .GroupBy(ol => ol.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(ol => ol.Quantity));

        var allLoadRequestLines = loadRequests
            .SelectMany(lr => lr.LoadRequestLines)
            .ToList();

        var loadRequestQuantities = allLoadRequestLines
            .GroupBy(lrl => lrl.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(lrl => lrl.Quantity));

        // Check if distinct product count matches
        if (orderedQuantities.Count != loadRequestQuantities.Count)
        {
            _logger.LogInformation(
                "Order {OrderId} product count mismatch. Ordered distinct products: {OrderedCount}, LoadRequest distinct products: {LoadCount}",
                orderId, orderedQuantities.Count, loadRequestQuantities.Count);
            return false;
        }

        // Check each product quantity matches exactly
        foreach (var (productId, expectedQty) in orderedQuantities)
        {
            if (!loadRequestQuantities.TryGetValue(productId, out var actualQty))
            {
                _logger.LogInformation("Order {OrderId}: ProductId {ProductId} is missing from LoadRequests", orderId, productId);
                return false;
            }

            if (actualQty != expectedQty)
            {
                _logger.LogInformation(
                    "Order {OrderId}: ProductId {ProductId} quantity mismatch. Ordered: {ExpectedQty}, LoadRequests Total: {ActualQty}",
                    orderId, productId, expectedQty, actualQty);
                return false;
            }
        }

        _logger.LogInformation(
            "Order {OrderId} satisfies all conditions (All {Count} LoadRequests verified and all quantities match). Generating BOM parts...",
            orderId, loadRequests.Count);

        // Action: For each load request and line, fetch ProductBOM and generate LoadRequestParts
        foreach (var loadRequest in loadRequests)
        {
            foreach (var line in loadRequest.LoadRequestLines)
            {
                // Remove existing parts for this line to prevent duplicates
                var existingParts = line.LoadRequestParts.ToList();
                foreach (var oldPart in existingParts)
                {
                    await _unitOfWork.LoadRequestParts.DeleteAsync(oldPart.Id);
                }
                line.LoadRequestParts.Clear();

                var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(line.ProductId);
                foreach (var bom in boms)
                {
                    var part = new LoadRequestPart
                    {
                        LoadRequestId = loadRequest.Id,
                        LoadRequest = loadRequest,
                        LoadRequestLineId = line.Id,
                        LoadRequestLine = line,
                        ProductId = line.ProductId,
                        PartId = bom.PartId,
                        RequiredQuantity = line.Quantity * bom.Quantity,
                        LoadedQuantity = 0,
                        Status = "Pending",
                        CreatedAt = DateTime.UtcNow
                    };

                    line.LoadRequestParts.Add(part);
                    loadRequest.LoadRequestParts.Add(part);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully generated LoadRequestParts for all LoadRequests in Order {OrderId}", orderId);
        return true;
    }
}
