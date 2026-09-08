using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.OrderLine;

public class GetOrderLinesByOrderIdQueryHandler : IRequestHandler<GetOrderLinesByOrderIdQuery, IEnumerable<OrderLineDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetOrderLinesByOrderIdQueryHandler> _logger;

    public GetOrderLinesByOrderIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetOrderLinesByOrderIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<OrderLineDto>> Handle(GetOrderLinesByOrderIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetOrderLinesByOrderIdQuery for OrderId: {OrderId}", request.OrderId);

        var orderLines = await _unitOfWork.OrderLines.GetByOrderIdAsync(request.OrderId);

        return orderLines.Select(ol => new OrderLineDto
        {
            Id = ol.Id,
            OrderId = ol.OrderId,
            OrderNumber = ol.Order?.OrderNumber,
            ProductVariantId = ol.ProductVariantId,
            ProductVariantCode = ol.ProductVariant?.Code,
            Quantity = ol.Quantity,
            Notes = ol.Notes,
            CreatedAt = ol.CreatedAt,
            UpdatedAt = ol.UpdatedAt
        }).ToList();
    }
}
