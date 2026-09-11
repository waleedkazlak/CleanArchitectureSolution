using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Order;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new CleanSample.Domain.Entities.Order
        {
            ClientId = request.ClientId,
            OrderDate = request.OrderDate ?? DateTime.UtcNow,
            RequiredDate = request.RequiredDate,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Draft" : request.Status,
            Notes = request.Notes,
            OrderLines = request.OrderLines.Select(l => new CleanSample.Domain.Entities.OrderLine
            {
                ProductId = l.ProductId,
                Quantity = l.Quantity,
                Notes = l.Notes,
                CreatedAt = DateTime.UtcNow
            }).ToList()
        };

        var orderId = await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return orderId;
    }
}
