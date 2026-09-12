using CleanSample.Domain.Enums;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Order;

public class ApproveOrderCommandHandler : IRequestHandler<ApproveOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveOrderCommandHandler> _logger;

    public ApproveOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<ApproveOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(ApproveOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sales manager approving Order with ID: {OrderId}", request.OrderId);

        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            _logger.LogWarning("Order with ID: {OrderId} not found for approval", request.OrderId);
            return false;
        }

        order.Status = (int)OrderStatusEnum.Approved;
        order.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Order {OrderId} successfully approved (Status = 3 Approved)", request.OrderId);
        return true;
    }
}
