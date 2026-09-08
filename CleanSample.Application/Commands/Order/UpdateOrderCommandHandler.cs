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
        var order = await _unitOfWork.Orders.GetByIdAsync(request.Id);
        if (order == null)
        {
            return false;
        }

        order.OrderNumber = request.OrderNumber;
        order.ClientId = request.ClientId;
        order.OrderDate = request.OrderDate;
        order.RequiredDate = request.RequiredDate;
        order.Status = request.Status;
        order.Notes = request.Notes;
        order.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
