using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.OrderLine;

public class DeleteOrderLineCommandHandler : IRequestHandler<DeleteOrderLineCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderLineCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteOrderLineCommand request, CancellationToken cancellationToken)
    {
        var orderLine = await _unitOfWork.OrderLines.GetByIdAsync(request.Id);
        if (orderLine == null)
        {
            return false;
        }

        var parentOrderId = orderLine.OrderId;
        await _unitOfWork.OrderLines.DeleteAsync(request.Id);

        // Update parent order status to Pending (2)
        var parentOrder = await _unitOfWork.Orders.GetByIdAsync(parentOrderId);
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
