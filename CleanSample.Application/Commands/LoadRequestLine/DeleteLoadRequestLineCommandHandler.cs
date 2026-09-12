using CleanSample.Application.Services;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.LoadRequestLine;

public class DeleteLoadRequestLineCommandHandler : IRequestHandler<DeleteLoadRequestLineCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoadRequestBOMService _loadRequestBOMService;
    private readonly ILogger<DeleteLoadRequestLineCommandHandler> _logger;

    public DeleteLoadRequestLineCommandHandler(
        IUnitOfWork unitOfWork,
        ILoadRequestBOMService loadRequestBOMService,
        ILogger<DeleteLoadRequestLineCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _loadRequestBOMService = loadRequestBOMService;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteLoadRequestLineCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting LoadRequestLine with Id: {Id}", request.Id);

        var existing = await _unitOfWork.LoadRequestLines.GetByIdAsync(request.Id);
        if (existing == null)
        {
            _logger.LogWarning("LoadRequestLine with Id: {Id} not found", request.Id);
            return false;
        }

        var loadRequestId = existing.LoadRequestId;

        await _unitOfWork.LoadRequestLines.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Check verification and quantities for associated Order
        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(loadRequestId);
        if (loadRequest != null && loadRequest.OrderId.HasValue)
        {
            await _loadRequestBOMService.CheckAndGeneratePartsForOrderAsync(loadRequest.OrderId.Value, cancellationToken);
        }

        _logger.LogInformation("LoadRequestLine with Id: {Id} deleted successfully", request.Id);
        return true;
    }
}
