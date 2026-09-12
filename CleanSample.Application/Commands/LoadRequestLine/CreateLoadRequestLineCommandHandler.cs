using CleanSample.Application.DTOs;
using CleanSample.Application.Services;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.LoadRequestLine;

public class CreateLoadRequestLineCommandHandler : IRequestHandler<CreateLoadRequestLineCommand, List<LoadRequestLineDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoadRequestBOMService _loadRequestBOMService;
    private readonly ILogger<CreateLoadRequestLineCommandHandler> _logger;

    public CreateLoadRequestLineCommandHandler(
        IUnitOfWork unitOfWork,
        ILoadRequestBOMService loadRequestBOMService,
        ILogger<CreateLoadRequestLineCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _loadRequestBOMService = loadRequestBOMService;
        _logger = logger;
    }

    public async Task<List<LoadRequestLineDto>> Handle(CreateLoadRequestLineCommand request, CancellationToken cancellationToken)
    {
        var itemsToProcess = new List<CreateLoadRequestLineItemDto>();

        if (request.Items != null && request.Items.Any())
        {
            itemsToProcess.AddRange(request.Items);
        }
        else if (request.LoadRequestId > 0 && request.ProductId > 0)
        {
            itemsToProcess.Add(new CreateLoadRequestLineItemDto
            {
                Id = request.Id,
                LoadRequestId = request.LoadRequestId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });
        }

        if (!itemsToProcess.Any())
        {
            _logger.LogWarning("CreateLoadRequestLineCommand called with no items to process");
            return new List<LoadRequestLineDto>();
        }

        _logger.LogInformation("Processing {Count} LoadRequestLine items (create/update)", itemsToProcess.Count);

        var processedIds = new List<long>();

        foreach (var item in itemsToProcess)
        {
            Domain.Entities.LoadRequestLine? existing = null;

            // 1. If an explicit Id > 0 is provided, find by Id
            if (item.Id.HasValue && item.Id.Value > 0)
            {
                existing = await _unitOfWork.LoadRequestLines.GetByIdAsync(item.Id.Value);
            }

            // 2. If not found by Id, check if a record with the same (LoadRequestId, ProductId) already exists
            if (existing == null)
            {
                existing = await _unitOfWork.LoadRequestLines.GetByLoadRequestAndProductIdAsync(item.LoadRequestId, item.ProductId);
            }

            if (existing != null)
            {
                var productChanged = existing.ProductId != item.ProductId;
                var quantityChanged = existing.Quantity != item.Quantity;

                existing.LoadRequestId = item.LoadRequestId;
                existing.ProductId = item.ProductId;
                existing.Quantity = item.Quantity;
                existing.UpdatedAt = DateTime.UtcNow;

                if (productChanged)
                {
                    // Remove old parts
                    var oldParts = existing.LoadRequestParts.ToList();
                    foreach (var oldPart in oldParts)
                    {
                        await _unitOfWork.LoadRequestParts.DeleteAsync(oldPart.Id);
                    }
                    existing.LoadRequestParts.Clear();

                    // Generate new parts based on Product BOM
                    var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(item.ProductId);
                    foreach (var bom in boms)
                    {
                        existing.LoadRequestParts.Add(new LoadRequestPart
                        {
                            LoadRequestId = existing.LoadRequestId,
                            LoadRequestLineId = existing.Id,
                            LoadRequestLine = existing,
                            ProductId = existing.ProductId,
                            PartId = bom.PartId,
                            RequiredQuantity = item.Quantity * bom.Quantity,
                            LoadedQuantity = 0,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
                else if (quantityChanged)
                {
                    // Recalculate required quantity based on BOM
                    var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(item.ProductId);
                    var bomDict = boms.ToDictionary(b => b.PartId, b => b.Quantity);

                    foreach (var part in existing.LoadRequestParts)
                    {
                        if (bomDict.TryGetValue(part.PartId, out var bomQty))
                        {
                            part.RequiredQuantity = item.Quantity * bomQty;
                            part.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                }

                await _unitOfWork.LoadRequestLines.UpdateAsync(existing);
                processedIds.Add(existing.Id);
                _logger.LogInformation("Updated existing LoadRequestLine with Id: {Id}", existing.Id);
            }
            else
            {
                // Create new record
                var newLine = new Domain.Entities.LoadRequestLine
                {
                    LoadRequestId = item.LoadRequestId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    CreatedAt = DateTime.UtcNow
                };

                // Generate parts based on Product BOM
                var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(item.ProductId);
                foreach (var bom in boms)
                {
                    newLine.LoadRequestParts.Add(new LoadRequestPart
                    {
                        LoadRequestId = item.LoadRequestId,
                        LoadRequestLine = newLine,
                        ProductId = item.ProductId,
                        PartId = bom.PartId,
                        RequiredQuantity = item.Quantity * bom.Quantity,
                        LoadedQuantity = 0,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                var newId = await _unitOfWork.LoadRequestLines.AddAsync(newLine);
                processedIds.Add(newId);
                _logger.LogInformation("Created new LoadRequestLine with Id: {Id}", newId);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // For all affected LoadRequests, check if their OrderId satisfies verification and quantity rules
        var affectedLoadRequestIds = itemsToProcess
            .Select(i => i.LoadRequestId)
            .Where(id => id > 0)
            .Distinct()
            .ToList();

        foreach (var loadRequestId in affectedLoadRequestIds)
        {
            var loadReq = await _unitOfWork.LoadRequests.GetByIdAsync(loadRequestId);
            if (loadReq != null && loadReq.OrderId.HasValue)
            {
                await _loadRequestBOMService.CheckAndGeneratePartsForOrderAsync(loadReq.OrderId.Value, cancellationToken);
            }
        }

        // Fetch full entity details with relations to map to DTOs
        var resultDtos = new List<LoadRequestLineDto>();
        foreach (var id in processedIds)
        {
            var fullLine = await _unitOfWork.LoadRequestLines.GetByIdAsync(id);
            if (fullLine != null)
            {
                resultDtos.Add(new LoadRequestLineDto
                {
                    Id = fullLine.Id,
                    LoadRequestId = fullLine.LoadRequestId,
                    ProductId = fullLine.ProductId,
                    ProductName = fullLine.Product?.Name,
                    Quantity = fullLine.Quantity,
                    CreatedAt = fullLine.CreatedAt,
                    UpdatedAt = fullLine.UpdatedAt,
                    LoadRequestParts = fullLine.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
                    {
                        Id = lrp.Id,
                        LoadRequestId = lrp.LoadRequestId,
                        LoadRequestLineId = lrp.LoadRequestLineId,
                        ProductId = lrp.ProductId,
                        ProductName = lrp.Product?.Name ?? fullLine.Product?.Name,
                        PartId = lrp.PartId,
                        PartCode = lrp.Part?.Code,
                        PartName = lrp.Part?.Name,
                        RequiredQuantity = lrp.RequiredQuantity,
                        LoadedQuantity = lrp.LoadedQuantity,
                        CreatedAt = lrp.CreatedAt,
                        UpdatedAt = lrp.UpdatedAt
                    }).ToList() ?? new List<LoadRequestPartDto>()
                });
            }
        }

        _logger.LogInformation("Successfully processed {Count} LoadRequestLine items", resultDtos.Count);
        return resultDtos;
    }
}
