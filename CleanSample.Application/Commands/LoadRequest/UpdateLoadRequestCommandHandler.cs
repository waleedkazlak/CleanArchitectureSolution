using CleanSample.Application.Services;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class UpdateLoadRequestCommandHandler : IRequestHandler<UpdateLoadRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoadRequestBOMService _loadRequestBOMService;

    public UpdateLoadRequestCommandHandler(
        IUnitOfWork unitOfWork,
        ILoadRequestBOMService loadRequestBOMService)
    {
        _unitOfWork = unitOfWork;
        _loadRequestBOMService = loadRequestBOMService;
    }

    public async Task<bool> Handle(UpdateLoadRequestCommand request, CancellationToken cancellationToken)
    {
        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(request.Id);
        if (loadRequest == null)
        {
            return false;
        }

        var previousOrderId = loadRequest.OrderId;

        loadRequest.OrderId = request.OrderId;
        loadRequest.ClientId = request.ClientId;
        loadRequest.ClientLocationId = request.ClientLocationId;
        loadRequest.RequestedBy = request.RequestedBy;
        loadRequest.RequestDate = request.RequestDate;
        loadRequest.ExecutionDate = request.ExecutionDate;
        loadRequest.Status = request.Status;
        loadRequest.DestinationAddress = request.DestinationAddress;
        loadRequest.DestinationCity = request.DestinationCity;
        loadRequest.Description = request.Description;
        loadRequest.DriverId = request.DriverId;
        loadRequest.VehicleId = request.VehicleId;
        loadRequest.Verified = request.Verified;
        loadRequest.UpdatedAt = DateTime.UtcNow;

        // Synchronize LoadRequestLines in-place on the tracked collection
        var incomingLineIds = request.LoadRequestLines
            .Where(l => l.Id > 0)
            .Select(l => l.Id)
            .ToHashSet();

        var linesToRemove = loadRequest.LoadRequestLines
            .Where(l => !incomingLineIds.Contains(l.Id))
            .ToList();

        foreach (var lineToRemove in linesToRemove)
        {
            loadRequest.LoadRequestLines.Remove(lineToRemove);
        }

        foreach (var incomingLine in request.LoadRequestLines)
        {
            if (incomingLine.Id > 0)
            {
                var existingLine = loadRequest.LoadRequestLines
                    .FirstOrDefault(l => l.Id == incomingLine.Id);

                if (existingLine != null)
                {
                    var productChanged = existingLine.ProductId != incomingLine.ProductId;
                    var quantityChanged = existingLine.Quantity != incomingLine.Quantity;

                    existingLine.ProductId = incomingLine.ProductId;
                    existingLine.Quantity = incomingLine.Quantity;
                    existingLine.UpdatedAt = DateTime.UtcNow;

                    if (productChanged)
                    {
                        var oldParts = existingLine.LoadRequestParts.ToList();
                        foreach (var oldPart in oldParts)
                        {
                            await _unitOfWork.LoadRequestParts.DeleteAsync(oldPart.Id);
                        }
                        existingLine.LoadRequestParts.Clear();

                        var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(incomingLine.ProductId);
                        foreach (var bom in boms)
                        {
                            var partEntity = new LoadRequestPart
                            {
                                LoadRequestId = loadRequest.Id,
                                LoadRequest = loadRequest,
                                LoadRequestLineId = existingLine.Id,
                                LoadRequestLine = existingLine,
                                ProductId = incomingLine.ProductId,
                                PartId = bom.PartId,
                                RequiredQuantity = incomingLine.Quantity * bom.Quantity,
                                LoadedQuantity = 0,
                                CreatedAt = DateTime.UtcNow
                            };
                            existingLine.LoadRequestParts.Add(partEntity);
                            loadRequest.LoadRequestParts.Add(partEntity);
                        }
                    }
                    else if (quantityChanged)
                    {
                        var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(incomingLine.ProductId);
                        var bomDict = boms.ToDictionary(b => b.PartId, b => b.Quantity);

                        foreach (var part in existingLine.LoadRequestParts)
                        {
                            if (bomDict.TryGetValue(part.PartId, out var bomQty))
                            {
                                part.RequiredQuantity = incomingLine.Quantity * bomQty;
                                part.UpdatedAt = DateTime.UtcNow;
                            }
                        }
                    }
                }
            }
            else
            {
                var newLine = new CleanSample.Domain.Entities.LoadRequestLine
                {
                    LoadRequestId = loadRequest.Id,
                    LoadRequest = loadRequest,
                    ProductId = incomingLine.ProductId,
                    Quantity = incomingLine.Quantity,
                    CreatedAt = DateTime.UtcNow
                };

                var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(incomingLine.ProductId);
                foreach (var bom in boms)
                {
                    var partEntity = new LoadRequestPart
                    {
                        LoadRequestId = loadRequest.Id,
                        LoadRequest = loadRequest,
                        LoadRequestLine = newLine,
                        ProductId = incomingLine.ProductId,
                        PartId = bom.PartId,
                        RequiredQuantity = incomingLine.Quantity * bom.Quantity,
                        LoadedQuantity = 0,
                        CreatedAt = DateTime.UtcNow
                    };
                    newLine.LoadRequestParts.Add(partEntity);
                    loadRequest.LoadRequestParts.Add(partEntity);
                }

                loadRequest.LoadRequestLines.Add(newLine);
            }
        }

        await _unitOfWork.LoadRequests.UpdateAsync(loadRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Check verification and quantities for current Order
        if (loadRequest.OrderId.HasValue)
        {
            await _loadRequestBOMService.CheckAndGeneratePartsForOrderAsync(loadRequest.OrderId.Value, cancellationToken);
        }

        // Also check previous Order if OrderId was changed
        if (previousOrderId.HasValue && previousOrderId != loadRequest.OrderId)
        {
            await _loadRequestBOMService.CheckAndGeneratePartsForOrderAsync(previousOrderId.Value, cancellationToken);
        }

        return true;
    }
}
