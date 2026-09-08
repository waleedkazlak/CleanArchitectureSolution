using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.OrderLine;

public class CreateOrderLineCommandHandler : IRequestHandler<CreateOrderLineCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderLineCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(CreateOrderLineCommand request, CancellationToken cancellationToken)
    {
        var orderLine = new CleanSample.Domain.Entities.OrderLine
        {
            OrderId = request.OrderId,
            ProductVariantId = request.ProductVariantId,
            Quantity = request.Quantity,
            Notes = request.Notes
        };

        var orderLineId = await _unitOfWork.OrderLines.AddAsync(orderLine);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return orderLineId;
    }
}
