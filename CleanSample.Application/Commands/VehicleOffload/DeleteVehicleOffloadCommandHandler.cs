using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleOffload;

public class DeleteVehicleOffloadCommandHandler : IRequestHandler<DeleteVehicleOffloadCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteVehicleOffloadCommandHandler> _logger;
    private readonly Services.IWorkflowOrchestratorService _workflowOrchestrator;

    public DeleteVehicleOffloadCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteVehicleOffloadCommandHandler> logger,
        Services.IWorkflowOrchestratorService workflowOrchestrator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _workflowOrchestrator = workflowOrchestrator;
    }

    public async Task<bool> Handle(DeleteVehicleOffloadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteVehicleOffloadCommand for ID: {VehicleOffloadId}", request.Id);

        var existingOffload = await _unitOfWork.VehicleOffloads.GetByIdAsync(request.Id);
        if (existingOffload == null)
        {
            _logger.LogWarning("VehicleOffload with ID {VehicleOffloadId} not found", request.Id);
            return false;
        }

        var loadRequestId = existingOffload.LoadRequestId;
        await _unitOfWork.VehicleOffloads.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Trigger workflow recalculation for LoadRequest
        await _workflowOrchestrator.RecalculateLoadRequestStatusAfterVehicleOffloadsChangeAsync(loadRequestId, cancellationToken);

        _logger.LogInformation("Successfully deleted VehicleOffload with ID: {VehicleOffloadId}", request.Id);
        return true;
    }
}
