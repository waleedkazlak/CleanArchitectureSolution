using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.OrderLine;

public class UpdateOrderLineCommandHandler : IRequestHandler<UpdateOrderLineCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderLineCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateOrderLineCommand request, CancellationToken cancellationToken)
    {
        var orderLine = await _unitOfWork.OrderLines.GetByIdAsync(request.Id);
        if (orderLine == null)
        {
            return false;
        }

        orderLine.OrderId = request.OrderId;
        orderLine.ProductId = request.ProductId;
        orderLine.Quantity = request.Quantity;
        orderLine.Notes = request.Notes;
        orderLine.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.OrderLines.UpdateAsync(orderLine);

        // Update parent order status to Pending (2)
        var parentOrder = await _unitOfWork.Orders.GetByIdAsync(orderLine.OrderId);
        if (parentOrder != null && parentOrder.Status != (int)Domain.Enums.OrderStatusEnum.Completed)
        {
            parentOrder.Status = (int)Domain.Enums.OrderStatusEnum.Pending;
            parentOrder.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Orders.UpdateAsync(parentOrder);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
