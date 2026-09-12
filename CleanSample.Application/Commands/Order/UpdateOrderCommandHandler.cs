using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Order;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var existingOrder = await _unitOfWork.Orders.GetByIdAsync(request.Id);
        if (existingOrder == null)
        {
            return false;
        }

        existingOrder.ClientId = request.ClientId;
        existingOrder.OrderDate = request.OrderDate;
        existingOrder.RequiredDate = request.RequiredDate;
        existingOrder.Status = request.Status > 0 ? request.Status : (int)CleanSample.Domain.Enums.OrderStatusEnum.Pending;
        existingOrder.Notes = request.Notes;
        existingOrder.UpdatedAt = DateTime.UtcNow;

        // Synchronize OrderLines in-place on the tracked collection
        var incomingLineIds = request.OrderLines
            .Where(l => l.Id > 0)
            .Select(l => l.Id)
            .ToHashSet();

        // 1. Remove lines not present in incoming request
        var linesToRemove = existingOrder.OrderLines
            .Where(l => !incomingLineIds.Contains(l.Id))
            .ToList();

        foreach (var lineToRemove in linesToRemove)
        {
            existingOrder.OrderLines.Remove(lineToRemove);
        }

        // 2. Update existing lines or add new lines
        foreach (var incomingLine in request.OrderLines)
        {
            if (incomingLine.Id > 0)
            {
                var existingLine = existingOrder.OrderLines
                    .FirstOrDefault(l => l.Id == incomingLine.Id);

                if (existingLine != null)
                {
                    existingLine.ProductId = incomingLine.ProductId;
                    existingLine.Quantity = incomingLine.Quantity;
                    existingLine.Notes = incomingLine.Notes;
                    existingLine.UpdatedAt = DateTime.UtcNow;
                }
            }
            else
            {
                existingOrder.OrderLines.Add(new CleanSample.Domain.Entities.OrderLine
                {
                    OrderId = existingOrder.Id,
                    ProductId = incomingLine.ProductId,
                    Quantity = incomingLine.Quantity,
                    Notes = incomingLine.Notes,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await _unitOfWork.Orders.UpdateAsync(existingOrder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
