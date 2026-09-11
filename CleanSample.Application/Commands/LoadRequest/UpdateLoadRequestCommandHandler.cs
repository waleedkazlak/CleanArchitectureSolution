using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class UpdateLoadRequestCommandHandler : IRequestHandler<UpdateLoadRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLoadRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateLoadRequestCommand request, CancellationToken cancellationToken)
    {
        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(request.Id);
        if (loadRequest == null)
        {
            return false;
        }

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
                                PartId = bom.PartId,
                                RequiredQuantity = incomingLine.Quantity * bom.Quantity,
                                LoadedQuantity = 0,
                                Status = "Pending",
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
                        PartId = bom.PartId,
                        RequiredQuantity = incomingLine.Quantity * bom.Quantity,
                        LoadedQuantity = 0,
                        Status = "Pending",
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

        return true;
    }
}
