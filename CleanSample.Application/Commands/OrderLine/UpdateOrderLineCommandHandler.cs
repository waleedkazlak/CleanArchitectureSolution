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
        orderLine.ProductVariantId = request.ProductVariantId;
        orderLine.Quantity = request.Quantity;
        orderLine.Notes = request.Notes;
        orderLine.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.OrderLines.UpdateAsync(orderLine);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
