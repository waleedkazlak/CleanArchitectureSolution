using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.OrderLine;

public class CreateOrderLineCommandHandler : IRequestHandler<CreateOrderLineCommand, List<OrderLineDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateOrderLineCommandHandler> _logger;

    public CreateOrderLineCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateOrderLineCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<OrderLineDto>> Handle(CreateOrderLineCommand request, CancellationToken cancellationToken)
    {
        var itemsToProcess = new List<CreateOrderLineItemDto>();

        if (request.Items != null && request.Items.Any())
        {
            itemsToProcess.AddRange(request.Items);
        }
        else if (request.OrderId > 0 && request.ProductId > 0)
        {
            itemsToProcess.Add(new CreateOrderLineItemDto
            {
                Id = request.Id,
                OrderId = request.OrderId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Notes = request.Notes
            });
        }

        if (!itemsToProcess.Any())
        {
            _logger.LogWarning("CreateOrderLineCommand called with no items to process");
            return new List<OrderLineDto>();
        }

        _logger.LogInformation("Processing {Count} OrderLine items (create/update)", itemsToProcess.Count);

        var processedIds = new List<long>();

        foreach (var item in itemsToProcess)
        {
            Domain.Entities.OrderLine? existing = null;

            // 1. If an explicit Id > 0 is provided, find by Id
            if (item.Id.HasValue && item.Id.Value > 0)
            {
                existing = await _unitOfWork.OrderLines.GetByIdAsync(item.Id.Value);
            }

            // 2. If not found by Id, check if a record with the same (OrderId, ProductId) already exists
            if (existing == null)
            {
                existing = await _unitOfWork.OrderLines.GetByOrderAndProductIdAsync(item.OrderId, item.ProductId);
            }

            if (existing != null)
            {
                // Update existing record
                existing.OrderId = item.OrderId;
                existing.ProductId = item.ProductId;
                existing.Quantity = item.Quantity;
                existing.Notes = item.Notes;
                existing.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.OrderLines.UpdateAsync(existing);
                processedIds.Add(existing.Id);
                _logger.LogInformation("Updated existing OrderLine with Id: {Id}", existing.Id);
            }
            else
            {
                // Create new record
                var newLine = new Domain.Entities.OrderLine
                {
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Notes = item.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                var newId = await _unitOfWork.OrderLines.AddAsync(newLine);
                processedIds.Add(newId);
                _logger.LogInformation("Created new OrderLine with Id: {Id}", newId);
            }
        }

        // Update parent order(s) status to Pending (2)
        var affectedOrderIds = itemsToProcess.Select(i => i.OrderId).Distinct().Where(id => id > 0);
        foreach (var orderId in affectedOrderIds)
        {
            var parentOrder = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (parentOrder != null && parentOrder.Status != (int)Domain.Enums.OrderStatusEnum.Completed)
            {
                parentOrder.Status = (int)Domain.Enums.OrderStatusEnum.Pending;
                parentOrder.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Orders.UpdateAsync(parentOrder);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fetch full entity details with relations to map to DTOs
        var resultDtos = new List<OrderLineDto>();
        foreach (var id in processedIds)
        {
            var fullLine = await _unitOfWork.OrderLines.GetByIdAsync(id);
            if (fullLine != null)
            {
                resultDtos.Add(new OrderLineDto
                {
                    Id = fullLine.Id,
                    OrderId = fullLine.OrderId,
                    ProductId = fullLine.ProductId,
                    ProductName = fullLine.Product?.Name,
                    Quantity = fullLine.Quantity,
                    Notes = fullLine.Notes,
                    CreatedAt = fullLine.CreatedAt,
                    UpdatedAt = fullLine.UpdatedAt
                });
            }
        }

        _logger.LogInformation("Successfully processed {Count} OrderLine items", resultDtos.Count);
        return resultDtos;
    }
}
