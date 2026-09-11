using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.OrderLine;

public class GetOrderLineByIdQueryHandler : IRequestHandler<GetOrderLineByIdQuery, OrderLineDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderLineByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderLineDto?> Handle(GetOrderLineByIdQuery request, CancellationToken cancellationToken)
    {
        var ol = await _unitOfWork.OrderLines.GetByIdAsync(request.Id);
        if (ol == null)
        {
            return null;
        }

        return new OrderLineDto
        {
            Id = ol.Id,
            OrderId = ol.OrderId,
            ProductId = ol.ProductId,
            ProductName = ol.Product?.Name,
            Quantity = ol.Quantity,
            Notes = ol.Notes,
            CreatedAt = ol.CreatedAt,
            UpdatedAt = ol.UpdatedAt
        };
    }
}
