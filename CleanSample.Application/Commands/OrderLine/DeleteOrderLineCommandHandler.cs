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

        await _unitOfWork.OrderLines.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
