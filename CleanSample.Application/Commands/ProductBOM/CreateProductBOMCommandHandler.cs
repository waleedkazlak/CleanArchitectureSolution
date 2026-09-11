using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.ProductBOM;

public class CreateProductBOMCommandHandler : IRequestHandler<CreateProductBOMCommand, List<ProductBOMDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProductBOMCommandHandler> _logger;

    public CreateProductBOMCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateProductBOMCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<ProductBOMDto>> Handle(CreateProductBOMCommand request, CancellationToken cancellationToken)
    {
        var itemsToProcess = new List<CreateProductBOMItemDto>();

        if (request.Items != null && request.Items.Any())
        {
            itemsToProcess.AddRange(request.Items);
        }
        else if (request.ProductId > 0 && request.PartId > 0)
        {
            itemsToProcess.Add(new CreateProductBOMItemDto
            {
                Id = request.Id,
                ProductId = request.ProductId,
                PartId = request.PartId,
                Quantity = request.Quantity
            });
        }

        if (!itemsToProcess.Any())
        {
            _logger.LogWarning("CreateProductBOMCommand called with no items to process");
            return new List<ProductBOMDto>();
        }

        _logger.LogInformation("Processing {Count} ProductBOM items (create/update)", itemsToProcess.Count);

        var processedIds = new List<int>();

        foreach (var item in itemsToProcess)
        {
            Domain.Entities.ProductBOM? existing = null;

            // 1. If an explicit Id > 0 is provided, find by Id
            if (item.Id.HasValue && item.Id.Value > 0)
            {
                existing = await _unitOfWork.ProductBOMs.GetByIdAsync(item.Id.Value);
            }

            // 2. If not found by Id, check if a record with the same (ProductId, PartId) already exists
            if (existing == null)
            {
                existing = await _unitOfWork.ProductBOMs.GetByProductAndPartIdAsync(item.ProductId, item.PartId);
            }

            if (existing != null)
            {
                // Update existing record
                existing.ProductId = item.ProductId;
                existing.PartId = item.PartId;
                existing.Quantity = item.Quantity;
                existing.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.ProductBOMs.UpdateAsync(existing);
                processedIds.Add(existing.Id);
                _logger.LogInformation("Updated existing ProductBOM with Id: {Id}", existing.Id);
            }
            else
            {
                // Create new record
                var newBom = new Domain.Entities.ProductBOM
                {
                    ProductId = item.ProductId,
                    PartId = item.PartId,
                    Quantity = item.Quantity,
                    CreatedAt = DateTime.UtcNow
                };

                var newId = await _unitOfWork.ProductBOMs.AddAsync(newBom);
                processedIds.Add(newId);
                _logger.LogInformation("Created new ProductBOM with Id: {Id}", newId);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fetch full entity details with relations to map to DTOs
        var resultDtos = new List<ProductBOMDto>();
        foreach (var id in processedIds)
        {
            var fullBom = await _unitOfWork.ProductBOMs.GetByIdAsync(id);
            if (fullBom != null)
            {
                resultDtos.Add(new ProductBOMDto
                {
                    Id = fullBom.Id,
                    ProductId = fullBom.ProductId,
                    ProductName = fullBom.Product?.Name,
                    PartId = fullBom.PartId,
                    PartCode = fullBom.Part?.Code,
                    PartName = fullBom.Part?.Name,
                    Quantity = fullBom.Quantity,
                    CreatedAt = fullBom.CreatedAt,
                    UpdatedAt = fullBom.UpdatedAt
                });
            }
        }

        _logger.LogInformation("Successfully processed {Count} ProductBOM items", resultDtos.Count);
        return resultDtos;
    }
}
