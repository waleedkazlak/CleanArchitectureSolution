using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.FieldAssembly;

public class DeleteFieldAssemblyCommandHandler : IRequestHandler<DeleteFieldAssemblyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteFieldAssemblyCommandHandler> _logger;
    private readonly Services.IWorkflowOrchestratorService _workflowOrchestrator;

    public DeleteFieldAssemblyCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteFieldAssemblyCommandHandler> logger,
        Services.IWorkflowOrchestratorService workflowOrchestrator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _workflowOrchestrator = workflowOrchestrator;
    }

    public async Task<bool> Handle(DeleteFieldAssemblyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteFieldAssemblyCommand for ID: {FieldAssemblyId}", request.Id);

        var existingAssembly = await _unitOfWork.FieldAssemblies.GetByIdAsync(request.Id);
        if (existingAssembly == null)
        {
            _logger.LogWarning("FieldAssembly with ID {FieldAssemblyId} not found", request.Id);
            return false;
        }

        var fieldJobId = existingAssembly.FieldJobId;
        await _unitOfWork.FieldAssemblies.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Check if all FieldAssemblies for the related LoadRequest are Completed (2)
        var job = await _unitOfWork.FieldJobs.GetByIdAsync(fieldJobId);
        if (job != null)
        {
            await _workflowOrchestrator.RecalculateLoadRequestStatusAfterFieldAssembliesChangeAsync(job.LoadRequestId, cancellationToken);
        }

        _logger.LogInformation("Successfully deleted FieldAssembly with ID: {FieldAssemblyId}", request.Id);
        return true;
    }
}
