using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Order;

public class GetOrdersByClientIdQueryHandler : IRequestHandler<GetOrdersByClientIdQuery, IEnumerable<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrdersByClientIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByClientIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.Orders.GetByClientIdAsync(request.ClientId);

        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            ClientId = o.ClientId,
            ClientName = o.Client?.Name,
            OrderDate = o.OrderDate,
            RequiredDate = o.RequiredDate,
            Status = o.Status,
            Notes = o.Notes,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt
        }).ToList();
    }
}
