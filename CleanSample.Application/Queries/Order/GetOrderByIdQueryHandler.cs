using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Order;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var o = await _unitOfWork.Orders.GetByIdAsync(request.Id);
        if (o == null)
        {
            return null;
        }

        return new OrderDto
        {
            Id = o.Id,
            ClientId = o.ClientId,
            ClientName = o.Client?.Name,
            OrderDate = o.OrderDate,
            RequiredDate = o.RequiredDate,
            Status = o.Status,
            StatusName = o.OrderStatus?.Name,
            Notes = o.Notes,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt,
            OrderLines = o.OrderLines?.Select(ol => new OrderLineDto
            {
                Id = ol.Id,
                OrderId = ol.OrderId,
                ProductId = ol.ProductId,
                ProductName = ol.Product?.Name,
                Quantity = ol.Quantity,
                Notes = ol.Notes,
                CreatedAt = ol.CreatedAt,
                UpdatedAt = ol.UpdatedAt
            }).ToList() ?? new List<OrderLineDto>()
        };
    }
}
