using CleanSample.Application.Services;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class DeleteLoadRequestCommandHandler : IRequestHandler<DeleteLoadRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoadRequestBOMService _loadRequestBOMService;

    public DeleteLoadRequestCommandHandler(
        IUnitOfWork unitOfWork,
        ILoadRequestBOMService loadRequestBOMService)
    {
        _unitOfWork = unitOfWork;
        _loadRequestBOMService = loadRequestBOMService;
    }

    public async Task<bool> Handle(DeleteLoadRequestCommand request, CancellationToken cancellationToken)
    {
        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(request.Id);
        if (loadRequest == null)
        {
            return false;
        }

        var orderId = loadRequest.OrderId;

        await _unitOfWork.LoadRequests.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (orderId.HasValue)
        {
            await _loadRequestBOMService.CheckAndGeneratePartsForOrderAsync(orderId.Value, cancellationToken);
        }

        return true;
    }
}
